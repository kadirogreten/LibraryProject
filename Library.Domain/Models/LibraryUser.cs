using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Library.Domain.Models
{
    public class LibraryUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.Now;

        public ICollection<BookRezervation> Rezervations { get; set; }

    }

    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<LibraryUser>
    {
        public CustomClaimsPrincipalFactory(UserManager<LibraryUser> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor) { }

        public async override Task<ClaimsPrincipal> CreateAsync(LibraryUser user)
        {
            var principal = await base.CreateAsync(user);

            // Add your claims here
            ((ClaimsIdentity)principal.Identity).AddClaims(
                new[] {
                    new Claim (ClaimTypes.Name, user.UserName),
                        new Claim (CustomClaimTypes.UserId, user.Id.ToString ())
                });

            return principal;
        }
    }

    public static class CustomClaimTypes
    {
        public const string UserId = "UserId";
    }
}
