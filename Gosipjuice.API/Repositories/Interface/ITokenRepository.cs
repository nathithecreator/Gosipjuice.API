using Microsoft.AspNetCore.Identity;

namespace Gosipjuice.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string>roles);
    }
}
