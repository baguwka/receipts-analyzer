using System.Collections.Generic;
using System.Threading.Tasks;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public interface IItemsRepository
    {
        Task<IReadOnlyCollection<Item>> GetItemsAsync();
        Task<Item> AddItem(Item item);
    }
}