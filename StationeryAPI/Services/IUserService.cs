
using StationeryAPI.ShoppingModels;

namespace demo_jwt_netcore.Services
{
    public interface IUserService
    {
        TblAdEmpAccount Authenticate(string username, string password);
        // void SaveRefreshToken(int userId, string refreshToken);
        // string GetRefreshToken(int userId);
    }
}
