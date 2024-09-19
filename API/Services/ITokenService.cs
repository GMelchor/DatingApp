using API.Entities;

namespace API.Services;

public class ITokenService
{
    string CreateToken(AppUser user);
}
