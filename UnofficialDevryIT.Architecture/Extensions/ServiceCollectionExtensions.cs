using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using UnofficialDevryIT.Architecture.Modules;

namespace UnofficialDevryIT.Architecture.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly IModuleConfigurationManager _modulesConfig = new ModuleConfigurationManager();

        public static IServiceCollection AddModules(this IServiceCollection services)
        {
            foreach (var module in _modulesConfig.GetModules())
            {
                if (!module.IsBundledWithHost)
                {
                    TryLoadModuleAssembly(module.Id, module);
                    if (module.Assembly == null)
                        throw new Exception($"Cannot find main assembly for module {module.Id}");
                }
                else
                {
                    module.Assembly = Assembly.Load(new AssemblyName(module.Id));
                }

                GlobalConfiguration.Modules.Add(module);
            }

            return services;
        }

        private static void TryLoadModuleAssembly(string moduleFolderPath, ModuleInfo module)
        {
            const string binariesFolderName = "bin";
            var binariesFolderPath = Path.Combine(moduleFolderPath, binariesFolderName);
            var binariesFolder = new DirectoryInfo(binariesFolderPath);

            if (Directory.Exists(binariesFolderPath))
            {
                foreach (var file in binariesFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                {
                    Assembly assembly;

                    try
                    {
                        assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                    }
                    catch (FileLoadException)
                    {
                        // get loaded assembly. This assembly might be loaded
                        assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));

                        if (assembly == null)
                            throw;

                        string loadedAssemblyVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
                        string tryToLoadAssemblyVersion = FileVersionInfo.GetVersionInfo(file.FullName).FileVersion;
                        
                        // or log the exception somewhere and don't add the module to list so that it will not be initialized
                        if (tryToLoadAssemblyVersion != loadedAssemblyVersion)
                            throw new Exception(
                                $"Cannot load {file.FullName} {tryToLoadAssemblyVersion} beacuse {assembly.Location} {loadedAssemblyVersion} has been loaded");
                    }

                    if (Path.GetFileNameWithoutExtension(assembly.ManifestModule.Name) == module.Id)
                        module.Assembly = assembly;
                }
            }
        }
    }
}