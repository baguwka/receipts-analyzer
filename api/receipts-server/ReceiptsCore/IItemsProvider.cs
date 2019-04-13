using System.Collections.Generic;
using System.Threading.Tasks;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public interface IItemsProvider
    {
        Task<IReadOnlyCollection<Item>> GetItemsAsync();
    }
}