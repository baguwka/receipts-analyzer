using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceiptsCore.EF.Model
{
    [Table("users")]
    public class User
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