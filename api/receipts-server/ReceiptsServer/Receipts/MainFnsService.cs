using System;
using System.Threading.Tasks;
using CheckReceiptSDK;
using CheckReceiptSDK.Results;
using ReceiptsCore;

namespace ReceiptsServer.Receipts
{
    public class MainFnsService : IFnsService
    {
        private readonly IFnsUsersRepository _UsersRepository;

        public MainFnsService(IFnsUsersRepository usersRepository)
        {
            _UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public Task<CheckResult> IsReceiptExists(string fiscalNumber, string fiscalDocument, string fiscalSign, DateTime date, decimal sum)
        {
            return FNS.CheckAsync(fiscalNumber, fiscalDocument, fiscalSign, date, sum);
        }

        public async Task<ReceiptResult> ReceiveAsync(string fiscalNumber, string fiscalDocument, string fiscalSign)
        {
            var user = await _UsersRepository.GetMainUserAsync();
            if (user == null)
                throw new FnsUserNotFoundException("Can't find user to authorize to FNS. Internal server problem. Check db.");

            return await FNS.ReceiveAsync(fiscalNumber, fiscalDocument, fiscalSign, user.Username, user.Password);
        }
    }
}