using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Xml;


namespace JMRSDK.EditorScript
{
    public enum PlatformType
    {
        SM = 0,
        CU
    }

    public static class ManifestConfig
    {
        private static string manifestPath = Path.Combine(Environment.CurrentDirectory + "/Assets/Plugins/Android/AndroidManifest.xml");

        public static PlatformType currentPlatform = PlatformType.SM;

        /// <summary>
        /// Completely Reset Android manifest [All permissions & attributes disabled]
        /// </summary>
        //[MenuItem("JioMixedReality/Manifest/ResetManifest", priority = 11)]
        public static void ResetAndroidManifest()
        {
            ResetPermissions();
            DisableRecentHistoryAttributes();

            Debug.Log("Status --> Android Manifest Reset");
        }

        /// <summary>
        /// Use to set the manifest to SM platform
        /// </summary>
        // [MenuItem("Update Manifest/SM Manifest Setup")]
        public static void SM_Setup()
        {
            currentPlatform = PlatformType.SM;
            ResetAndroidManifest();
            EnableRecentHistoryAttributes();

            Debug.Log("Status --> SM Manifest Setup Completed");
        }

        /// <summary>
        /// Use to set the manifest to CU platform
        /// </summary>
        //  [MenuItem("Update Manifest/CU Manifest setup")]
        public static void CU_Setup()
        {
            currentPlatform = PlatformType.CU;
            ResetAndroidManifest();
            DisableRecentHistoryAttributes();

            Debug.Log("Status --> CU Manifest Setup Completed");
        }

        #region PERMISSION EDITOR

        static string disabledCamera = $"<!--<uses-permission android:name=\"android.permission.CAMERA\"/>-->";
        static string enabledCamera = $"<uses-permission android:name=\"android.permission.CAMERA\"/>";

        static string disabledAudio = $"<!--<uses-permission android:name=\"android.permission.RECORD_AUDIO\"/>-->";
        static string enabledAudio = $"<uses-permission android:name=\"android.permission.RECORD_AUDIO\"/>";

        static string disableWriteExternalPermission = $"<uses-permission android:name=\"android.permission.WRITE_EXTERNAL_STORAGE\" tools:node=\"remove\" />";
        static string enableWriteExternalPermission = $"<uses-permission android:name=\"android.permission.WRITE_EXTERNAL_STORAGE\"/>";

        static string disableReadExternalPermission = $"<uses-permission android:name=\"android.permission.READ_EXTERNAL_STORAGE\" tools:node=\"remove\" />";
        static string enableReadExternalPermission = $"<uses-permission android:name=\"android.permission.READ_EXTERNAL_STORAGE\"/>";

        /// <summary>
        /// Toggles Audio and Camera permission at the same time [Recommended to use this method]
        /// By Default Permissions should be disabled
        /// </summary>
        //  [MenuItem("Update Manifest/Toggle Camera Audio Permission")]
        public static void ToggleCameraAudioPrmission()
        {
            TogglePermissions(enabledCamera, disabledCamera);
            TogglePermissions(enabledAudio, disabledAudio);
        }

        /// <summary>
        /// Toggles Read and write external storage permission at the same time [Recommended to use this method]
        /// By Default Permissions should be disabled
        /// </summary>
        //  [MenuItem("Update Manifest/Toggle Storage Permission")]
        public static void ToggleStoragePermissions()
        {
            TogglePermissions(enableWriteExternalPermission, disableWriteExternalPermission);
            TogglePermissions(enableReadExternalPermission, disableReadExternalPermission);
        }

        /// <summary>
        /// Remove all mentioned permissions
        /// [Camera,Audio,Write external,Read External]
        /// </summary>
        //[MenuItem("JioMixedReality/Manifest/ResetPermissions", priority = 12)]
        public static void ResetPermissions()
        {
            TogglePermissions(enabledCamera, disabledCamera, true);
            TogglePermissions(enabledAudio, disabledAudio, true);

            TogglePermissions(enableWriteExternalPermission, disableWriteExternalPermission, true);
            TogglePermissions(enableReadExternalPermission, disableReadExternalPermission, true);
        }

        public static void TogglePermissions(string enableString, string disableString, bool isReset = false)
        {
            string manifest = ReadString(manifestPath);

            if (string.IsNullOrEmpty(manifest))
            {
                Debug.LogError("Manifest not found."); return;
            }

            if (isReset)
            {
                if (manifest.Contains(disableString))
                {
                    //Debug.LogWarning("Status -->  Permission already disabled");
                    return;
                }
                else if (manifest.Contains(enableString))
                {
                    manifest = manifest.Replace(enableString, disableString);
                    Debug.Log("Status -->  Permission RESET");
                }
            }
            else
            {
                //*** Dont reverse these conditions , otherwise this toggel wont work properly ***
                if (manifest.Contains(disableString))
                {
                    manifest = manifest.Replace(disableString, enableString);
                    Debug.Log("Status -->  Permission Enabled");
                }
                else if (manifest.Contains(enableString))
                {
                    manifest = manifest.Replace(enableString, disableString);
                    Debug.Log("Status -->  Permission Disabled");
                }
            }
            WriteString(manifest, manifestPath);
        }
        #endregion

        #region Utilities

        static string ReadString(string readPath = "")
        {
            StreamReader reader = new StreamReader(readPath);
            string textInFile = reader.ReadToEnd();
            reader.Close();
            return textInFile;
        }

        static void WriteString(string text, string writePath = "")
        {
            StreamWriter writer = new StreamWriter(writePath);
            writer.Write(text);
            writer.Close();
        }

        #endregion

        #region XML ATTRIBUTE UPDATE

        static string xmlString = string.Empty;
        private const string xmlPath = "Assets/Plugins/Android/AndroidManifest.xml";

        private const string xmlActivityNodePath = "manifest/application/activity";

        private const string NoHistory = "android:noHistory";
        private const string ExcludeFromRecents = "android:excludeFromRecents";

        private const string xmlMetaDataNotePath = "//manifest/application/meta-data";
        private const string attributeKey_PRO_LITE = "com.jiotesseract.platform";
        private const string AndroidValue = "android:value";
        private const string AndroidName = "android:name";
        private const string PRO = "PRO";
        private const string LITE = "LITE";

        public static void DisableRecentHistoryAttributes()
        {
            UpdateXMLNonRecurringAttributes(xmlActivityNodePath, NoHistory, false.ToString());
            UpdateXMLNonRecurringAttributes(xmlActivityNodePath, ExcludeFromRecents, false.ToString());
        }

        public static void EnableRecentHistoryAttributes()
        {
            UpdateXMLNonRecurringAttributes(xmlActivityNodePath, NoHistory, true.ToString());
            UpdateXMLNonRecurringAttributes(xmlActivityNodePath, ExcludeFromRecents, true.ToString());
        }

        [MenuItem("JioMixedReality/Manifest/Configure for PRO")]
        public static void ConfigForPRO()
        {
            UpdateXMLRecurringAttributes(xmlMetaDataNotePath, AndroidValue, PRO, AndroidName, attributeKey_PRO_LITE);
            CU_Setup();
        }

        [MenuItem("JioMixedReality/Manifest/Configure for LITE")]
        public static void ConfigForLITE()
        {
            UpdateXMLRecurringAttributes(xmlMetaDataNotePath, AndroidValue, LITE, AndroidName, attributeKey_PRO_LITE);
            SM_Setup();
        }
        /// <summary>
        /// For unique node attributes (which doesnt repeat in manifest)
        /// </summary>
        /// <param name="xmlNodePath"></param>
        /// <param name="xmlAttributeName"></param>
        /// <param name="value"></param>
        private static void UpdateXMLNonRecurringAttributes(string xmlNodePath, string xmlAttributeName, string value)
        {
            xmlString = File.ReadAllText(xmlPath);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            //var element = doc.SelectSingleNode(xmlNodePath) as XmlElement;
            var list = doc.SelectNodes(xmlNodePath);

            foreach (XmlElement element in list)
            {
                if (element != null && element.HasAttribute(xmlAttributeName))
                {
                    element.SetAttribute(xmlAttributeName, value.ToString());
                    doc.Save(xmlPath);
                }
                else
                {
                    Debug.LogError("XML Element " + xmlAttributeName + " not found. Please check the PATH");
                }
            }
        }

        /// <summary>
        /// For re-curring node attributes (which repeats in manifest)
        /// </summary>
        /// <param name="xmlNodePath"></param>
        /// <param name="xmlTargetAttributeName"></param>
        /// <param name="value"></param>
        /// <param name="xmlSearchAttribute"></param>
        /// <param name="xmlSearchAttributeKey"></param>
        private static void UpdateXMLRecurringAttributes(string xmlNodePath, string xmlTargetAttributeName, string value, string xmlSearchAttribute, string xmlSearchAttributeKey)
        {
            xmlString = File.ReadAllText(xmlPath);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            var list = doc.SelectNodes(xmlNodePath);

            foreach (XmlElement element in list)
            {
                if (element != null && element.HasAttribute(xmlSearchAttribute) && element.HasAttribute(xmlTargetAttributeName))
                {
                    if (element.Attributes[xmlSearchAttribute].Value == xmlSearchAttributeKey)
                    {
                        element.SetAttribute(xmlTargetAttributeName, value.ToString());
                        doc.Save(xmlPath);
                    }
                }
                else
                {
                    Debug.LogError("XML Element not found. Please check the PATH");
                }
            }
        }

        #endregion
    }
}