namespace NativoChallenge.Domain.Interfaces;

public interface IEmailSender 
{
    public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}