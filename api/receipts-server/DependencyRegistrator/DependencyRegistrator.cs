using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Receipts.Core;
using Receipts.Core.Contract;
using Receipts.Framework;
using Receipts.Logic.Contract.Hash;
using Receipts.Logic.Contract.Logging;
using Receipts.Logic.Contract.Receipts;
using Receipts.Logic.Hash;
using Receipts.Logic.Logging;
using Receipts.Logic.Receipts;
using ReceiptsCore;
using IReceiptsProvider = Receipts.Logic.Contract.Receipts.IReceiptsProvider;

namespace DependencyRoot
{
    public static class DependencyRegistrator
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<ILogger>(provider => provider.GetService<ILogger<ApplicationLogs>>());
            
            services.AddTransient<IFnsUsersRepository, DbFnsUsersRepository>();
            services.AddTransient<IItemsRepository, DbItemsRepository>();
            services.AddTransient<IReceiptsRepository, DbReceiptsRepository>();

            services.AddTransient<IHashCalculator, JsonToMd5HashCalculator>();
            services.AddTransient<IFnsService, MainFnsService>();
            services.AddTransient<IReceiptsProvider, DefaultReceiptsProvider>();
            services.AddTransient<IRequestBodyLogger, AspRequestBodyLogger>();
        }
    }
}