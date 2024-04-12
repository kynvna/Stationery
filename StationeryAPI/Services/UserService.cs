
using StationeryAPI.ShoppingModels;

namespace demo_jwt_netcore.Services;

public class UserService:IUserService
{
    /*private readonly List<TblAdEmpAccount> _users = new List<TblAdEmpAccount>
    {
        
    };*/

    public TblAdEmpAccount Authenticate(string username, string password)
    {
        ShoppingWebContext db = new ShoppingWebContext();
        var user =db.TblAdEmpAccounts.FirstOrDefault(x => x.Username == username && x.Passw == password);
        return user;
    }
}