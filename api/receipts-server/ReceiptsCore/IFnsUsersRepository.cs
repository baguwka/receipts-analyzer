using System.Threading.Tasks;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public interface IFnsUsersRepository
    {
        Task<FnsUser> GetMainUserAsync();
    }
}