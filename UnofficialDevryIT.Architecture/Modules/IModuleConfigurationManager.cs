using System.Collections.Generic;

namespace UnofficialDevryIT.Architecture.Modules
{
    public interface IModuleConfigurationManager
    {
        IEnumerable<ModuleInfo> GetModules();
    }
}