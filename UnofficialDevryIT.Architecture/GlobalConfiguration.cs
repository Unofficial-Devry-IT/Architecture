using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnofficialDevryIT.Architecture.Modules;

namespace UnofficialDevryIT.Architecture
{
    public static class GlobalConfiguration
    {
        public static IList<ModuleInfo> Modules { get; set; } = new Collection<ModuleInfo>();
        public static string ContentRootPath { get; set; }
        
    }
}