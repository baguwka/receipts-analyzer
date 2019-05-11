using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceiptsCore.EF.Model
{
    [Table("receipts_extended")]
    public class ReceiptExtended
    {
        [Key]
        [Column("receipt_id")]
        public int ReceiptId { get; set; }

        [Column("cashier")]
        public string Cashier { get; set; }

        [Column("kkt_reg_id")]
        public string KktRegId { get; set; }

        [Column("retail_inn")]
        public string RetailInn { get; set; }
        
        [Column("retail_address")]
        public string RetailAddress { get; set; }

        [Column("shift_number")]
        public string ShiftNumber { get; set; }

        [Column("store_name")]
        public string StoreName { get; set; }
    }
}