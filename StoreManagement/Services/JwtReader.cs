using StoreManagement.Models;
using System.Security.Claims;

namespace StoreManagement.Services
{
    public class JwtReader
    {
        public static int getUserId(ClaimsPrincipal user)
        {
            int id;
            var Identity = user.Identity as ClaimsIdentity;
            if (Identity == null)
            {
                return -1;

            }
            var claim = Identity.Claims.FirstOrDefault(c => c.Type.ToLower() == "id");

            if (claim == null)
            {
                return -1;

            }

            try
            {
                id = int.Parse(claim.Value);
            }
            catch (Exception)
            {
                return -1;
            }
            return id;
        }

        public static string getUserRole(ClaimsPrincipal user)
        {
            var Identity = user.Identity as ClaimsIdentity;
            if (Identity == null)
            {
                return "";
            }

            var role = Identity.Claims.FirstOrDefault(x => x.Type.ToLower() == "role");
            if (role == null)
            {
                return "";
            }

            return role.Value;
        }

        public static Dictionary<string , string> GetClaims(ClaimsPrincipal user)
        {
            Dictionary<string , string> claims = new Dictionary<string , string>();
            var Identity = user.Identity as ClaimsIdentity;

            if (Identity != null)
            {
                foreach ( var claim in Identity.Claims)
                {
                    claims.Add(claim.Type , claim.Value);
                }
            }

            return claims;
        }
    }
}
