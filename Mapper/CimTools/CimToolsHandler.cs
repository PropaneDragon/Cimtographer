using CimTools;
using CimTools.V1;
using System.Reflection;

namespace Mapper.CimTools
{
    public static class CimToolsHandler
    {
        public static CimToolBase CimToolBase = new CimToolBase(new CimToolSettings("Mapper", Assembly.GetExecutingAssembly(), 549792340));
    }
}
