using System.Collections.Generic;
using System.Threading.Tasks;
using Receipts.Core.Contract.EF.Model;

namespace Receipts.Core.Contract
{
    public interface IItemsRepository
    {
        Task<IReadOnlyCollection<Item>> GetItemsAsync();
        Task<Item> AddItem(Item item);
    }
}