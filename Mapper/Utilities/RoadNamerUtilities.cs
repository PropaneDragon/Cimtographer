using ColossalFramework;
using ColossalFramework.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mapper.Utilities
{
    /// <summary>
    /// Utilities to interact with the Road Name Mod
    /// </summary>
    public static class RoadNamerUtilities
    {
        /// <summary>
        /// checks whether or not the Road Namer mod is loaded 
        /// </summary>
        /// <returns>Whether or not the mod(and the nessecary managers), are actually available</returns>
        public static bool HaveMod()
        {
            bool roadNamerModAvailable = false;
            roadNamerModAvailable = Singleton<PluginManager>.instance.GetPluginsInfo().Where(x => x.isEnabled && x.publishedFileID.AsUInt64 == 558960454UL).Count() == 1;
            return roadNamerModAvailable;
        }

        public static void testMethod()
        {
            if( HaveMod())
            {
                try
                {
                    var nameManagerClassList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.IsClass && x.Name == "RoadNameManager" && x.Namespace == "RoadNamer.Managers").ToList();
                    if (nameManagerClassList.Count == 1)
                    {
                        var nameManagerClass = nameManagerClassList[0];
                        object instance = nameManagerClass.GetMethod("Instance", BindingFlags.Public | BindingFlags.Static).Invoke(null,null);
                        DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, "RoadManager is :" + instance);

                        if (instance != null)
                        {

                            MemberInfo[] propertyInfos = nameManagerClass.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                            if( propertyInfos != null)
                            {
                               
                                foreach (MemberInfo propertyInfo in propertyInfos)
                                {
                                    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, "roadManager property:" + propertyInfo.Name);
                                }
                            }
                            

                            //TODO:Remove( along with the entire method, I guess)
                            var testList = instance.GetType().GetProperty("m_roadList", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).GetValue(instance, null) as List<Object>;
                            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, "# of names in RoadManager :" + testList.Count);
                        }


                    }
                }
                catch (Exception e)
                {
                    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, e.ToString());

                }
            }
        }
    }
}
