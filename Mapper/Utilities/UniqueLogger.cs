using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mapper.Utilities
{
    static class UniqueLogger
    {
        private static Dictionary<string, Dictionary<string, string>> logs = new Dictionary<string, Dictionary<string, string>>();

        public static void AddLog(string logKey, string key, string value)
        {
            if(!logs.ContainsKey(logKey))
            {
                logs[logKey] = new Dictionary<string, string>();
            }
            
            logs[logKey][key] = value;
        }

        public static void PrintLog(string logKey)
        {
            string outputLog = "";

            if (logs.ContainsKey(logKey))
            {
                outputLog += logKey + " - - - - - -\n";

                List<string> sortedKeys = logs[logKey].Keys.ToList();
                sortedKeys.Sort();

                foreach (string key in sortedKeys)
                {
                    outputLog += key + ": " + logs[logKey][key] + "\n";
                }

                Debug.Log(outputLog);
            }
        }
    }
}
