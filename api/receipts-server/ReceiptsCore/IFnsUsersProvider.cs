using System.Threading.Tasks;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public interface IFnsUsersProvider
    {
        Task<FnsUser> GetMainUserAsync();
        Task<FnsUser> GetUserForTestsAsync();
    }
}