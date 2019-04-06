﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ReceiptsApiServer
{
    public class CustomWebHostService : WebHostService
    {
        private ILogger _Logger;

        public CustomWebHostService(IWebHost host) : base(host)
        {
            _Logger = host.Services
                .GetRequiredService<ILogger<CustomWebHostService>>();
        }

        protected override void OnStarting(string[] args)
        {
            _Logger.LogInformation("ScreenMatcher service starting...");
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            _Logger.LogInformation("ScreenMatcher service started");
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            _Logger.LogInformation("ScreenMatcher service stopping...");
            base.OnStopping();
        }

        protected override void OnStopped()
        {
            _Logger.LogInformation("ScreenMatcher service stopped");
            base.OnStopped();
        }
    }
}