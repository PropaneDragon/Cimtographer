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

        /// <summary>
        /// Finds, via reflection, the Road Namer mod classes relevant to the Cimtographer mod( roadNameManager ), and load the required methods 
        /// </summary>
        /// <returns>Boolean value representing whether or not the RoadNamer classes were found and loaded into the RoadNameManager instance</returns>
        public bool populateObjects()
        {
            if (HaveMod())
            {
                try
                {
                    //Attempt to find the RoadNameManager and RoadContainer classes within the current loaded assemblies
                    var roadNameManagerClass = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.IsClass && x.Name == "RoadNameManager" && x.Namespace == "RoadNamer.Managers").Single();
                    var roadContainerClass = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.IsClass && x.Name == "RoadContainer" && x.Namespace == "RoadNamer.Managers").Single();

                    if (roadNameManagerClass != null && roadContainerClass != null)
                    {
                        //If the classes were found, then get the singleton instance of the roadNameManager
                        roadNamerInstance = roadNameManagerClass.GetMethod("Instance", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
                        if (roadNamerInstance != null)
                        {
                            //If the instance was found, populate the RoadNamerManager method fields with the methods 
                            getRoadNameMethod = roadNamerInstance.GetType().GetMethod("GetRoadName", BindingFlags.Public | BindingFlags.Instance);

                            #if DEBUG
                            MemberInfo[] propertyInfos = roadNamerInstance.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                            if (propertyInfos != null)
                            {

                                foreach (MemberInfo propertyInfo in propertyInfos)
                                {
                                    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, string.Format("roadManager property/type:{0}/{1}", propertyInfo.Name, propertyInfo.MemberType));
                                }
                            }
                            #endif
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

        /// <summary>
        /// Wrapper for the "GetRoadName" method in the RoadNamer mod, invoked via reflection
        /// </summary>
        /// <param name="segmentId">the segment ID to search the names for</param>
        /// <returns>The name asscociated with the segment, or null if not found</returns>
        public string getSegmentName(ushort segmentId)
        {
            string returnValue = null;

            //If calling, mod classes should be available, but check again anyways
            bool getRoadNameMethodAvailable = false;
            if (getRoadNameMethod != null)
            {
                getRoadNameMethodAvailable = true;
            }
            else
            {
                getRoadNameMethodAvailable = populateObjects();
            }

            //If they are, then invoke the "GetRoadName" method, and get the returned name
            if (getRoadNameMethodAvailable)
            {
                returnValue = getRoadNameMethod.Invoke(roadNamerInstance, new object[] { segmentId }) as string;
            }
            return returnValue;
        }
    }


}
