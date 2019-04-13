using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceiptsServer.EF.Model
{
    [Table("items")]
    public class Item
    {
        [Key]
        [Column("item_id")]
        public int ItemId { get; set; }

        [Column("receipt_id")]
        public int ReceiptId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("price")]
        public int Price { get; set; }

        [Column("sum")]
        public int Sum { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }
    }
}