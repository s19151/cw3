using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cwiczenia3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        private string _path = "requestsLog.txt";

        public LoggingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            using (var writer = File.AppendText(_path)) {

                var request = context.Request;
                request.EnableBuffering();
                var bodyText = await new StreamReader(request.Body).ReadToEndAsync();
                request.Body.Position = 0;

                StringBuilder sb = new StringBuilder();

                var enumer = context.Request.Query.GetEnumerator();
                while (enumer.MoveNext()) {
                    var curr = enumer.Current;
                    sb.Append('\t');
                    sb.Append(curr.Key);
                    sb.Append('=');
                    sb.Append(curr.Value);
                }

                writer.Write($"{context.Request.Method}\n");
                writer.Write($"{context.Request.Path}\n");
                writer.Write($"{bodyText}\n");

                if(sb.Length > 0)
                    writer.Write($"{sb.ToString()}\n");

                writer.Write("\n");
            }

            await _next(context);
        }
    }
}
