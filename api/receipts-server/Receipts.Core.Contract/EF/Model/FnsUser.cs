using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Receipts.Core.Contract.EF.Model
{
    [Table("fns_users")]
    public class FnsUser
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("key")]
        public string Key { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }
    }
}