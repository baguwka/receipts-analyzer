﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceiptsCore.EF.Model
{
    [Table("receipts")]
    public class Receipt
    {
        [Key]
        [Column("receipt_id")]
        public int ReceiptId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("raw_qr_data")]
        public string RawQrData { get; set; }

        [Column("hash")]
        public string Hash { get; set; }

        public List<Item> Items { get; set; }
    }
}