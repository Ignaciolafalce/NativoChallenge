using MediatR;
using NativoChallenge.Domain.Interfaces;
using NativoChallenge.Application.Tasks.DTOs;
using NativoChallenge.Application.Tasks.Helpers;
using AutoMapper;
using System.ComponentModel;

namespace NativoChallenge.Application.Tasks.Queries.Handlers;

public class ListTasksQueryHandler : IRequestHandler<ListTasksQuery, ListTasksResult>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public ListTasksQueryHandler(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<ListTasksResult> Handle(ListTasksQuery query, CancellationToken cancellationToken)
    {
        var stateFilter = TaskFiltersHelper.GetStateFilter(query.State);
        var orderFunc = TaskOrderingHelper.GetOrderFunc(query.OrderBy);

        var tasks = await _taskRepository.GetAllAsync(stateFilter, cancellationToken);
        var orderedTasks = orderFunc(tasks.AsQueryable());

        var taskDtos = _mapper.Map<List<TaskDto>>(orderedTasks.ToList()); // automapper added :)
        //var taskDtos = orderedTasks.Select(t => new TaskDto(
        //                                        t.Id,
        //                                        t.Title,
        //                                        t.Description!,
        //                                        t.ExpirationDate,
        //                                        t.Priority.ToString(),
        //                                        t.State.ToString()))
        //                            .ToList();

        var warnings = new List<string>();
        var highPriorityPendingCount = await _taskRepository.CountHighPriorityPendingAsync(cancellationToken);
        var highPriorityPendingWarning = TaskWarningsHelpers.GetHighPriorityPendingWarningAsync(highPriorityPendingCount);
        if (highPriorityPendingWarning is not null)
        {
            warnings.Add(highPriorityPendingWarning ?? "Unknow warning.");
        }

        return new ListTasksResult(taskDtos, warnings);
    }
}
