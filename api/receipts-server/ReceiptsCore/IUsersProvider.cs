using System.Threading.Tasks;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public interface IUsersProvider
    {
        Task<User> GetMainUserAsync();
        Task<User> GetUserForTestsAsync();
    }
}