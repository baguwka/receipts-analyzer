using Newtonsoft.Json;
using ReceiptsCore.EF.Model;

namespace ReceiptsServer.Receipts
{
    public class ExtendedReceiptModel
    {
        [JsonProperty("cashier")]
        public string Cashier { get; set; }

        [JsonProperty("kkt_reg_id")]
        public string KktRegId { get; set; }

        [JsonProperty("retail_inn")]
        public string RetailInn { get; set; }

        [JsonProperty("retail_address")]
        public string RetailAddress { get; set; }

        [JsonProperty("store_number")]
        public string ShiftNumber { get; set; }

        [JsonProperty("store_name")]
        public string StoreName { get; set; }

        public static ExtendedReceiptModel FromDbModel(ReceiptExtended receiptExtended)
        {
            if (receiptExtended == null)
                return null;

            return new ExtendedReceiptModel
            {
                Cashier = receiptExtended.Cashier,
                KktRegId = receiptExtended.KktRegId,
                RetailAddress = receiptExtended.RetailAddress,
                RetailInn = receiptExtended.RetailInn,
                ShiftNumber = receiptExtended.ShiftNumber,
                StoreName = receiptExtended.StoreName
            };
        }
    }
}