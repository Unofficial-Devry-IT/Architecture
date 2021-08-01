using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace UnofficialDevryIT.Architecture.Extensions
{
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Adds all JSON files within the specified directory to the app's configuration
        /// Accessible via <see cref="IConfiguration"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static IWebHostBuilder AddJsonDirectoryToConfig(this IWebHostBuilder builder, string directoryPath)
        {
            builder.ConfigureAppConfiguration((host, context) =>
            {
                if (!Directory.Exists(directoryPath))
                    throw new DirectoryNotFoundException($"Could not locate JSON Directory at {directoryPath}");

                foreach (string file in Directory.GetFiles(directoryPath))
                {
                    FileInfo info = new FileInfo(file);

                    if (info.Extension.EndsWith("json"))
                        context.AddJsonFile(file);
                }
            });

            return builder;
        }
    }
}