using System.Threading.Tasks;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public interface IUsersProvider
    {
        Task<FnsUser> GetMainUserAsync();
        Task<FnsUser> GetUserForTestsAsync();
    }
}