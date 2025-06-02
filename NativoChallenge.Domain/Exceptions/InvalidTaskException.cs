namespace NativoChallenge.Domain.Exceptions
{
    [Serializable]
    public class InvalidTaskException : BusinessRuleException
    {
        public InvalidTaskException()
        {
        }

        public InvalidTaskException(string? message) : base(message)
        {
        }

        public InvalidTaskException(string? message, BusinessRuleException? innerException) : base(message, innerException)
        {
        }
    }
}
