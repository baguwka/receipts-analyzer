using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json;
using Receipts.Core.Contract.EF.Model;

namespace Receipts.Logic.Contract.Receipts
{
    public class ReceiptModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("raw_qr")]
        public string RawQRData { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("extended")]
        public ExtendedReceiptModel Extended { get; set; }

        [JsonProperty("user")]
        public UserModel User { get; set; }

        [JsonProperty("items")]
        public IReadOnlyCollection<ItemModel> Items { get; set; }

        public static ReceiptModel FromDbReceipt(Receipt receipt)
        {
            return new ReceiptModel
            {
                Id = receipt.ReceiptId,
                Date = receipt.Date,
                RawQRData = receipt.RawQrData,
                Hash = receipt.Hash,
                Extended = ExtendedReceiptModel.FromDbModel(receipt.Extended),
                Items = receipt.Items
                    .Select(ItemModel.FromDbModel)
                    .Where(i => i != null)
                    .ToImmutableList(),
                User = UserModel.FromDbModel(receipt.User)
            };
        }
    }
}