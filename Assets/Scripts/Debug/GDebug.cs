using System;
using UnityEngine;

namespace Log.GDebug
{
    public class GDebug
    {
        private const bool debugIn = false;

        public static void Log(string message, object sender = null, string category = null)
        {
            if (debugIn)
            {
                Debug.Log(GetCategory(category) + GetSenderType(sender) + message);
            }
        }

        public static void LogWarning(string message, object sender = null, string category = null)
        {
            if (debugIn)
            {
                Debug.LogWarning(GetCategory(category) + GetSenderType(sender) + message);
            }
        }

        public static void LogError(string message, object sender = null, string category = null)
        {
            if (debugIn)
            {
                Debug.LogError(GetCategory(category) + GetSenderType(sender) + message);
            }
        }

        public static void LogFatalError(string message, object sender = null, string category = null)
        {
            if (debugIn)
            {
                Debug.LogError(GetCategory(category) + GetSenderType(sender) + message);
            }
        }

        private static string GetCategory(string category)
        {
            return String.IsNullOrEmpty(category) ? "[UNKNOWN]" : category;
        }

        private static string GetSenderType(object sender)
        {
            if (sender != null)
                return "[" + sender.GetType().ToString() + "] ";
            else
                return "";
        }
    }
}
