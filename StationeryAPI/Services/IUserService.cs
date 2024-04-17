using StationeryAPI.ShoppingModels;

namespace StationeryAPI.Services
{
    public interface IUserService
    {
        TblAdEmpAccount Authenticate(string username, string password);
        // void SaveRefreshToken(int userId, string refreshToken);
        // string GetRefreshToken(int userId);
    }
}
