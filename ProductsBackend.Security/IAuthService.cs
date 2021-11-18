using System.Collections.Generic;
using ProductsBackend.Security.Model;

namespace ProductsBackend.Security
{
    public interface IAuthService
    {
        string GenerateJwtToken(LoginUser userUserName);
        string Hash(string password);
        List<Permission> GetPermissions(int userId);
    }
}