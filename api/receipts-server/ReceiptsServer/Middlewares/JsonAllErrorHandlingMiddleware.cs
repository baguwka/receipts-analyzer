using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ReceiptsServer.Receipts.Exceptions;

namespace ReceiptsServer.Middlewares
{
    public class JsonAllErrorHandlingMiddleware
    {
        private readonly RequestDelegate _Next;

        public JsonAllErrorHandlingMiddleware(RequestDelegate next)
        {
            _Next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is OverLimitException)
                code = HttpStatusCode.PaymentRequired;

            if (exception is FailedToAddReceiptException)
                code = HttpStatusCode.InternalServerError;

            if (exception is FnsProbablyUnavailableException)
                code = HttpStatusCode.ServiceUnavailable;

            if (exception is FnsUserNotFoundException)
                code = HttpStatusCode.Unauthorized;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var errorObject = JsonConvert.SerializeObject(new
            {
                error = "Error occured",
                status = (int)code,
                message = GetExceptionMessages(exception)
            });

            return context.Response.WriteAsync(errorObject);
        }

        private string GetExceptionMessages(Exception e, string msgs = "")
        {
            if (e == null) return string.Empty;
            if (msgs == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "\r\n" + GetExceptionMessages(e.InnerException);
            return msgs;
        }

    }
}