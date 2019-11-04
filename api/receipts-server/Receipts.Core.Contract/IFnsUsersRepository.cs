using System.Threading.Tasks;
using Receipts.Core.Contract.EF.Model;

namespace Receipts.Core.Contract
{
    public interface IFnsUsersRepository
    {
        Task<FnsUser> GetMainUserAsync();
    }
}