using UnityEngine;
using System;

#if (UNITY_ANDROID && !UNITY_EDITOR)
using System.Text;
#elif (UNITY_IOS && !UNITY_EDITOR)
using System.Runtime.InteropServices;
using UnityEngine.iOS;
#endif

namespace Lofelt.NiceVibrations
{
    /// <summary>
    /// C# wrapper for the Lofelt Studio Android and iOS SDK
    /// </summary>
    ///
    /// You should not use this class directly, use <c>HapticController</c> instead, or the
    /// <c>MonoBehaviour</c> classes <c>HapticReceiver</c> and <c>HapticSource</c>.
    ///
    /// The Lofelt Studio Android and iOS SDK are included in Nice Vibrations as pre-compiled
    /// binary plugins.
    ///
    /// Each method here delegates to either the Android or iOS SDK. The methods should only be
    /// called if <c>DeviceMeetsMinimumPlatformRequirements()</c> returns true, otherwise there will
    /// be runtime errors.
    ///
    /// All the methods do nothing when running in the Unity editor.
    ///
    /// Before calling any other method, <c>Initialize()</c> needs to be called.
    ///
    /// Errors are printed and swallowed, no exceptions are thrown. On iOS, this happens inside
    /// the SDK, on Android this happens with try/catch blocks in this class.
    public static class LofeltHaptics
    {
#if (UNITY_ANDROID && !UNITY_EDITOR)
        static AndroidJavaObject lofeltHaptics;
#elif (UNITY_IOS && !UNITY_EDITOR)
        // imports of iOS Framework bindings

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsDeviceMeetsMinimumRequirementsBinding();

        [DllImport("__Internal")]
        private static extern IntPtr lofeltHapticsInitBinding();

        // Use Marshalling to convert string into a pointer to a null-terminated char array.
        [DllImport("__Internal")]
        private static extern bool lofeltHapticsLoadBinding(IntPtr controller, [MarshalAs(UnmanagedType.LPStr)] string data);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsPlayBinding(IntPtr controller);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsStopBinding(IntPtr controller);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsSeekBinding(IntPtr controller, float time);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsSetAmplitudeMultiplicationBinding(IntPtr controller, float factor);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsSetFrequencyShiftBinding(IntPtr controller, float shift);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsLoopBinding(IntPtr controller, bool enable);

        [DllImport("__Internal")]
        private static extern float lofeltHapticsGetClipDurationBinding(IntPtr controller);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsReleaseBinding(IntPtr controller);

        static IntPtr controller = IntPtr.Zero;
#endif

        /// <summary>
        /// Initializes the iOS framework or Android library plugin
        /// </summary>
        ///
        /// This needs to be called before calling any other method.
        public static void Initialize()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var context = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    lofeltHaptics = new AndroidJavaObject("com.lofelt.haptics.LofeltHaptics", context);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            InitializeIosController();
#endif
        }

        /// <summary>
        /// Same as <c>Initialize()</c>, but for iOS only
        /// </summary>
        public static void InitializeIosController()
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            controller = lofeltHapticsInitBinding();
#endif
        }

        /// <summary>
        /// Releases the resources used by the iOS framework or Android library plugin
        /// </summary>
        public static void Release()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Dispose();
                lofeltHaptics = null;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            ReleaseIosController();
#endif
        }

        /// <summary>
        /// Same as <c>Release()</c>, but for iOS only
        /// </summary>
        public static void ReleaseIosController()
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            if(controller != IntPtr.Zero) {
                lofeltHapticsReleaseBinding(controller);
                controller = IntPtr.Zero;
            }
#endif
        }

        public static bool DeviceMeetsMinimumPlatformRequirements()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                return lofeltHaptics.Call<bool>("deviceMeetsMinimumRequirements");
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
                return false;
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            return lofeltHapticsDeviceMeetsMinimumRequirementsBinding();
#elif (UNITY_EDITOR)
            return true;
#endif
        }

        public static void Load(string data)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Call("load", Encoding.UTF8.GetBytes(data));
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsLoadBinding(controller, data);
#endif
        }

        public static float GetClipDuration()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                return lofeltHaptics.Call<float>("getClipDuration");
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
                return 0.0f;
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            return lofeltHapticsGetClipDurationBinding(controller);
#elif (UNITY_EDITOR)
            //No haptic clip was loaded with Lofelt SDK, so it returns 0.0f
            return 0.0f;
#endif
        }

        public static void Play()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Call("play");
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsPlayBinding(controller);
#endif
        }

        public static void Stop()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Call("stop");
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsStopBinding(controller);
#endif
        }

        public static void Seek(float time)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Call("seek", time);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsSeekBinding(controller, time);
#endif
        }

        public static void SetAmplitudeMultiplication(float factor)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Call("setAmplitudeMultiplication", factor);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsSetAmplitudeMultiplicationBinding(controller, factor);
#endif
        }

        public static void SetFrequencyShift(float shift)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsSetFrequencyShiftBinding(controller, shift);
#endif
        }

        public static void Loop(bool enabled)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Call("loop", enabled);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsLoopBinding(controller, enabled);
#endif
        }
    }
}
