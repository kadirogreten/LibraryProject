using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.API.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoleExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="roles"></param>
        public static void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
        }
    }
}
