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
    }
}
