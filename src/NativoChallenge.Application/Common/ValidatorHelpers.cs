namespace NativoChallenge.Application.Common;

public static class ValidatorHelpers
{
    public static bool IsGuidType(string field)
    {
        var parseResult = Guid.TryParse(field, out _);
        return parseResult;
    }
}
