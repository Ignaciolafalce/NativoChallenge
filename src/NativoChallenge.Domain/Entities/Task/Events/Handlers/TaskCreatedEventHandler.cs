using MediatR;
using Microsoft.Extensions.Logging;
using NativoChallenge.Domain.Interfaces;

namespace NativoChallenge.Domain.Entities.Task.Events.Handlers
{
    public class TaskCreatedEventHandler : INotificationHandler<TaskCreatedEvent>
    {

        private readonly ILogger<TaskCreatedEventHandler> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IMediator _mediator;

        public TaskCreatedEventHandler(IMediator mediator, ILogger<TaskCreatedEventHandler> logger, IEmailSender emailSender)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async System.Threading.Tasks.Task Handle(TaskCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling TaskCreatedEvent {@notification} for task {TaskId}", notification, notification.Task.Id);

            // Send email notification
            var to = "example@example.com";
            var subject = $"New Task - {notification.Task.Title}";
            var body = "A new Task was created WoW!";

            _logger.LogInformation("Sending email to {To} with subject {Subject}", to, subject);
            await _emailSender.SendEmailAsync(to, subject, body, cancellationToken);
        }
    }

}
