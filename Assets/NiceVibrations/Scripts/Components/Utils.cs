#if (UNITY_ANDROID && !UNITY_EDITOR)
    using UnityEngine;
#elif (UNITY_IOS && !UNITY_EDITOR)
    using UnityEngine.iOS;
#endif

namespace Lofelt.NiceVibrations
{
    /// <summary>
    /// A class containing utility functions that don't invoke haptics with
    /// <c>LofeltHaptics</c>
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Returns the iOS version on iOS, and 0 otherwise
        /// </summary>
        public static int IOSVersion()
        {
            int iOSVersion = 0;
#if (UNITY_IOS && !UNITY_EDITOR)
            string versionString = Device.systemVersion;
            string[] versionArray = versionString.Split('.');
            int.TryParse(versionArray[0], out iOSVersion);
#endif
            return iOSVersion;
        }

        /// <summary>
        /// Returns the Android API level on Android, and 0 otherwise
        /// </summary>
        public static int AndroidSDKVersion()
        {
            int androidVersion = 0;
#if (UNITY_ANDROID && !UNITY_EDITOR)
            androidVersion = int.Parse(SystemInfo.operatingSystem.Substring(SystemInfo.operatingSystem.IndexOf("-") + 1, 3));
#endif
            return androidVersion;
        }

        /// <summary>
        /// Indicates if the OS version is high enough to play haptics with Nice Vibrations
        /// </summary>
        ///
        /// The minimum required versions are:
        /// - iOS >= 13
        /// - Android API level >= 26
        public static bool IsVersionSupported()
        {
            bool supported = false;
#if (UNITY_ANDROID && !UNITY_EDITOR)
            const int minimumSupportedAndroidSDKVersion = 26;
            supported = AndroidSDKVersion() >= minimumSupportedAndroidSDKVersion;
#elif (UNITY_IOS && !UNITY_EDITOR)
            const int minimumSupportedIOSVersion = 13;
            supported = IOSVersion() >= minimumSupportedIOSVersion;
#elif (UNITY_EDITOR)
            supported = true;
#endif
            return supported;
        }
    }
}
