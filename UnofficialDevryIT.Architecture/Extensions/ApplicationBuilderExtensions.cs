using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using UnofficialDevryIT.Architecture.Modules;

namespace UnofficialDevryIT.Architecture.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder InitializeModules(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var moduleInitializers = app.ApplicationServices.GetServices<IModuleInitializer>();
            foreach (var moduleInitializer in moduleInitializers)
            {
                moduleInitializer.Configure(app, env);
            }

            return app;
        }
    }
}