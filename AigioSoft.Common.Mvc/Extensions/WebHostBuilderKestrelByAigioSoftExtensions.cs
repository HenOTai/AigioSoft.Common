using System;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Server.Kestrel;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Hosting
{
    public static class WebHostBuilderKestrelByAigioSoftExtensions
    {
        /// <summary>
        /// Specify Kestrel as the server to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The Microsoft.AspNetCore.Hosting.IWebHostBuilder to configure.</param>
        /// <param name="options">A callback to configure Kestrel options.</param>
        /// <param name="serverHeader">Set HTTP Response Header Server Value.</param>
        /// <returns>The Microsoft.AspNetCore.Hosting.IWebHostBuilder.</returns>
        public static IWebHostBuilder UseKestrel(this IWebHostBuilder hostBuilder,
            Action<KestrelServerOptions> options, string serverHeader)
        {
            ChangeValue(serverHeader);
            return hostBuilder.UseKestrel(options);
        }

        /// <summary>
        /// Specify Kestrel as the server to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The Microsoft.AspNetCore.Hosting.IWebHostBuilder to configure.</param>
        /// <param name="serverHeader">Set HTTP Response Header Server Value.</param>
        /// <returns>The Microsoft.AspNetCore.Hosting.IWebHostBuilder.</returns>
        public static IWebHostBuilder UseKestrel(this IWebHostBuilder hostBuilder, string serverHeader)
        {
            ChangeValue(serverHeader);
            return hostBuilder.UseKestrel();
        }

        private static void ChangeValue(string serverHeader)
        {
            if (string.IsNullOrWhiteSpace(serverHeader))
                throw new ArgumentNullException(nameof(serverHeader));
            var field = typeof(AspNetCore.Server.Kestrel.Internal.Http.Frame)
                .GetField("_bytesServer", BindingFlags.Static | BindingFlags.NonPublic);
            var defaultValue = field.GetValue(field) as byte[];
            if (defaultValue != null)
            {
                var defaultStr = Encoding.ASCII.GetString(defaultValue);
                if (defaultStr != null)
                {
                    defaultStr = defaultStr.Replace("Kestrel", serverHeader);
                    field.SetValue(field, Encoding.ASCII.GetBytes(defaultStr));
                }
            }
        }
    }
}
