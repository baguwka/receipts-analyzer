using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Receipts.Logic.Contract.Hash;
using ReceiptsCore.Utils;

namespace Receipts.Logic.Contract
{
    public class ReceiptRequestDto : IHashable
    {
        [JsonProperty("t")]
        public string TimeRaw { get; set; }

        [JsonIgnore]
        public DateTime Time { get; set; }

        [JsonProperty("s")]
        public decimal Sum { get; set; }

        [JsonProperty("fn")]
        public string FiscalNumber { get; set; }

        [JsonProperty("i")]
        public string FiscalDocument { get; set; }

        [JsonProperty("fp")]
        public string FiscalSign { get; set; }

        [JsonProperty("n")]
        public string N { get; set; }

        [JsonIgnore]
        public string RawQRData { get; set; }

        public bool IsValid() =>
            Time != default(DateTime) &&
            Sum > 0 &&
            !string.IsNullOrWhiteSpace(TimeRaw) &&
            !string.IsNullOrWhiteSpace(FiscalDocument) &&
            !string.IsNullOrWhiteSpace(FiscalNumber) &&
            !string.IsNullOrWhiteSpace(FiscalSign) &&
            !string.IsNullOrWhiteSpace(N);

        protected bool Equals(ReceiptRequestDto other)
        {
            return string.Equals(TimeRaw, other.TimeRaw) && Sum == other.Sum &&
                   string.Equals(FiscalNumber, other.FiscalNumber) &&
                   string.Equals(FiscalDocument, other.FiscalDocument) && 
                   string.Equals(FiscalSign, other.FiscalSign) &&
                   string.Equals(N, other.N);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ReceiptRequestDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (TimeRaw != null ? TimeRaw.GetDeterministicHashCode() : 0);
                hashCode = (hashCode * 397) ^ Sum.GetHashCode();
                hashCode = (hashCode * 397) ^ (FiscalNumber != null ? FiscalNumber.GetDeterministicHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FiscalDocument != null ? FiscalDocument.GetDeterministicHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FiscalSign != null ? FiscalSign.GetDeterministicHashCode() : 0);
                hashCode = (hashCode * 397) ^ (N != null ? N.GetDeterministicHashCode() : 0);
                return hashCode;
            }
        }
    }
}