using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access
{
    public abstract class JwtTokenHandler
    {
        protected SymmetricSecurityKey SecurityKey(string siginingKey)
        {
            var bytes = Encoding.UTF8.GetBytes(siginingKey);
            return new SymmetricSecurityKey(bytes);
        }
    }
}
