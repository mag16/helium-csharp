using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace Helium
{
    /// <summary>
    /// Registers aspnet middleware handler that handles default home page requests
    /// </summary>
    public static class HomePageMiddlewareExtensions
    {
        // list of paths to handle
        static readonly HashSet<string> validPaths = new HashSet<string> { "/", "/INDEX.HTML", "/INDEX.HTM", "/DEFAULT.HTML", "/DEFAULT.HTM" };

        // response to return
        static readonly byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes("Under construction ...");

        /// <summary>
        /// Middleware extension method to handle home page request
        /// </summary>
        /// <param name="builder">this IApplicationBuilder</param>
        /// <returns>IApplicationBuilder</returns>
        public static IApplicationBuilder UseHomePage(this IApplicationBuilder builder)
        {
            // implement the middleware
            builder.Use(async (context, next) =>
            {
                // matches / or index.htm[l] or default.htm[l]
                if (validPaths.Contains(context.Request.Path.Value.ToUpperInvariant()))
                {
                    // return the content
                    context.Response.ContentType = "text/html";
                    await context.Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length).ConfigureAwait(false);
                }
                else
                {
                    // not a match, so call next middleware handler
                    await next().ConfigureAwait(false);
                }
            });

            return builder;
        }
    }
}
