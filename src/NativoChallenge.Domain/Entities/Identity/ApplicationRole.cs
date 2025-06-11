using Microsoft.AspNetCore.Identity;

namespace NativoChallenge.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }

    }

}
