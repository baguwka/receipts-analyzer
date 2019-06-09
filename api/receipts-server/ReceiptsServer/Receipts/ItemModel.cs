using Newtonsoft.Json;
using ReceiptsCore.EF.Model;

namespace ReceiptsServer.Receipts
{
    public class ItemModel
    {
        [JsonProperty("id")]
        public int ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("sum")]
        public int Sum { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        public static ItemModel FromDbModel(Item item)
        {
            if (item == null)
                return null;

            return new ItemModel
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Sum = item.Sum,
                Price = item.Price,
                Quantity = item.Quantity
            };
        }
    }
}