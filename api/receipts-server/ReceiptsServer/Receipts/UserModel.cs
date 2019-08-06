using Newtonsoft.Json;
using ReceiptsCore.EF.Model;

namespace ReceiptsServer.Receipts
{
    public class UserModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        public static UserModel FromDbModel(User receiptUser)
        {
            if (receiptUser == null)
                return null;

            return new UserModel
            {
                Id = receiptUser.Id,
                Username = receiptUser.Username
            };
        }
    }
}