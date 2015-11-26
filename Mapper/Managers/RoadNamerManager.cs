using ColossalFramework;
using ColossalFramework.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mapper.Managers
{
    /// <summary>
    /// Utilities to interact with the Road Name Mod
    /// </summary>
    public class RoadNamerManager
    {

        private static RoadNamerManager instance = null;

        private Type roadNameManagerClass = null;
        private Type roadContainerClass = null;
        private object roadNamerInstance = null;
        private MethodInfo getRoadNameMethod = null;

        public static RoadNamerManager Instance()
        {
            if (instance == null)
            {
                instance = new RoadNamerManager();
            }

            return instance;
        }

        /// <summary>
        /// checks whether or not the Road Namer mod is loaded 
        /// </summary>
        /// <returns>Whether or not the mod(and the nessecary managers), are actually available</returns>
        public bool HaveMod()
        {
            bool roadNamerModAvailable = false;
            roadNamerModAvailable = Singleton<PluginManager>.instance.GetPluginsInfo().Where(x => x.isEnabled && x.publishedFileID.AsUInt64 == 558960454UL).Count() == 1;
            return roadNamerModAvailable;
        }

        public bool populateObjects()
        {
            if (HaveMod())
            {
                try
                {
                    var nameManagerClassList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.IsClass && x.Name == "RoadNameManager" && x.Namespace == "RoadNamer.Managers").ToList();
                    var roadContainerClassList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.IsClass && x.Name == "RoadContainer" && x.Namespace == "RoadNamer.Managers").ToList();

                    if (nameManagerClassList.Count == 1 && roadContainerClassList.Count == 1)
                    {
                        roadNameManagerClass = nameManagerClassList[0];
                        roadContainerClass = roadContainerClassList[0];
                        roadNamerInstance = roadNameManagerClass.GetMethod("Instance", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);

                        if (roadNamerInstance != null)
                        {
                            getRoadNameMethod = roadNamerInstance.GetType().GetMethod("GetRoadName", BindingFlags.Public | BindingFlags.Instance);
                            MemberInfo[] propertyInfos = roadNamerInstance.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                            if (propertyInfos != null)
                            {

                                foreach (MemberInfo propertyInfo in propertyInfos)
                                {
                                    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, string.Format("roadManager property/type:{0}/{1}", propertyInfo.Name, propertyInfo.MemberType));
                                }
                            }
                            return getRoadNameMethod != null;
                        }
                    }
                }
                catch (Exception e)
                {
                    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, e.ToString());
                }
            }
            return false;

        }

        public string getSegmentName(ushort segmentId)
        {
            string returnValue = null;
            bool getRoadNameMethodAvailable = false;
            if (getRoadNameMethod != null)
            {
                getRoadNameMethodAvailable = true;
            }
            else
            {
                getRoadNameMethodAvailable = populateObjects();
            }
            if (getRoadNameMethodAvailable)
            {
                returnValue = getRoadNameMethod.Invoke(roadNamerInstance, new object[] { segmentId }) as string;
            }
            return returnValue;
        }
    }

 
}
