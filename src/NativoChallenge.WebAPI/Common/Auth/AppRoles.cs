namespace NativoChallenge.WebAPI.Common.Auth
{
    public class AppRoles
    {
        public const string Admin = "Admin";
        public const string Guest = "Guest";

        public static readonly IReadOnlyList<string> All = [Admin, Guest];
    }

}
