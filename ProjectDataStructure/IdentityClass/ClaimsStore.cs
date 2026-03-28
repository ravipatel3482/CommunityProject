using System.Collections.Generic;
using System.Security.Claims;

namespace ProjectDataStructure.IdentityClass
{
    public class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
    {
        new Claim("Create-Post", "Create-Post"),
        new Claim("Edit-Post","Edit-Post"),
        new Claim("Delete-Post","Delete-Post")
    };
    }
}
