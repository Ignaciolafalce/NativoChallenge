using MediatR;
using Microsoft.Extensions.Logging;
using NativoChallenge.Domain.Interfaces;

namespace NativoChallenge.Domain.Entities.Task.Events.Handlers
{
    public class TaskCreatedEventHandler : INotificationHandler<TaskCreatedEvent>
    {

        private readonly ILogger<TaskCreatedEventHandler> _logger;
        private readonly IEmailSender _emailSender;

        public TaskCreatedEventHandler(ILogger<TaskCreatedEventHandler> logger, IEmailSender emailSender)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async System.Threading.Tasks.Task Handle(TaskCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling TaskCreatedEvent for task {TaskId}", notification.Task.Id);

            // Send email notification
            var to = "example@example.com";
            var subject = $"New Task - {notification.Task.Title}";
            var body = "A new Task was created WoW!";
            await _emailSender.SendEmailAsync(to, subject, body, cancellationToken);
        }
    }

}
