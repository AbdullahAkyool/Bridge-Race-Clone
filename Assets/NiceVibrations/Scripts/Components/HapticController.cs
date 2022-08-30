using UnityEngine;
using System;
using System.Timers;

#if (UNITY_ANDROID && !UNITY_EDITOR)
using System.Text;
#elif (UNITY_IOS && !UNITY_EDITOR)
using UnityEngine.iOS;
#endif

namespace Lofelt.NiceVibrations
{
    /// <summary>
    /// Provides haptic playback functionality
    /// </summary>
    ///
    /// <c>HapticController</c> allows you to load and play <c>.haptic</c> clips, and
    /// provides various ways to control playback, such as seeking, looping and
    /// amplitude/frequency modulation.
    ///
    /// If you need a <c>MonoBehaviour</c> API, use <c>HapticSource</c> and
    /// <c>HapticReceiver</c> instead.
    ///
    /// Compared to <c>LofeltHaptics</c>, <c>HapticController</c> adds additional API.
    ///
    /// None of the methods here are thread-safe and should only be called from the main (Unity)
    /// thread.
    public static class HapticController
    {
        static bool lofeltHapticsInitalized = false;

        // Timer used to call HandleFinishedPlayback() when playback is complete
        static Timer playbackFinishedTimer = new Timer();

        // Duration of the loaded haptic clip
        static float clipLoadedDuration = 0.0f;

        // JSON string of the clip previously loaded
        static string previousClipLoadedData = null;

        // The value of the last call to seek()
        static float lastSeekTime = 0.0f;

        // Previous amplitude multiplication value used when Reload() is called
        static float previousAmplitudeMultiplicationValue = 1.0f;

        // Previous frequency shift value used when Reload() is called
        static float previousFrequencyShift = 0.0f;

        // Flag indicating if the device supports the minimum requirements
        static bool deviceMeetsMinimumRequirements = false;

        // Flag indicating if playback looping is enabled
        static bool isLoopingEnabled = false;

        // Flag indicating if the playback is looping
        static bool isPlaybackLooping = false;

        internal static bool _hapticsEnabled = true;

        /// <summary>
        /// Property to enable and disable global haptic playback
        /// </summary>
        public static bool hapticsEnabled
        {
            get { return _hapticsEnabled; }
            set
            {
                if (_hapticsEnabled)
                {
                    Stop();
                }
                _hapticsEnabled = value;
            }
        }

        internal static float _hapticLevel = 1.0f;

        /// <summary>
        /// A global amplitude multiplication / haptic level that persists loading a new clip
        /// </summary>
        ///
        /// See <c>SetAmplitudeMultiplication()<c> for further details.
        ///
        /// Unlike the multiplication factor set by <c>SetAmplitudeMultiplication()<c>, the global
        /// haptic level is not reset to the default when loading a new clip.
        [System.ComponentModel.DefaultValue(1.0f)]
        public static float hapticLevel
        {
            get { return _hapticLevel; }
            set
            {
                _hapticLevel = value;

                // Set the global amplitude multiplication to _hapticLevel right away, and then
                // give subscribers of the HapticLevelChanged Action a chance to override it.
                SetAmplitudeMultiplication(_hapticLevel);
                HapticLevelChanged?.Invoke();
            }
        }

        /// Action that is invoked when <c>Load()</c> is called
        public static Action LoadedClipChanged;

        /// Action that is invoked when <c>Play()</c> is called
        public static Action PlaybackStarted;

        /// Action that is invoked when the playback has finished
        ///
        /// This happens either when <c>Stop()</c> is explicitly called, or when a non-looping
        /// clip has finished playing.
        ///
        /// This can be invoked spuriously, even if no haptics are currently playing, for example
        /// if <c>Stop()</c> is called multiple times in a row.
        public static Action PlaybackStopped;

        /// Action that is invoked when <c>hapticLevel</c> is changed
        public static Action HapticLevelChanged;

        /// <summary>
        /// Initializes <c>HapticController</c> and returns whether the device supports the minimum
        /// requirements
        /// </summary>
        ///
        /// Calling this method multiple times has no effect and is safe.
        ///
        /// You do not need to call this method, <c>HapticController</c> automatically calls this
        /// method before any operation that needs initialization, such as <c>Play</c>.
        /// However it can be beneficial to call this early during startup, so the initialization
        /// time is spent at startup instead of when the first haptic is triggered during gameplay.
        /// If you have a <c>HapticReceiver</c> in your scene, it takes care of calling
        /// <c>Init()</c> during startup for you.
        ///
        /// Do not call this method from a static constructor. Unity often invokes static
        /// constructors from a different thread, for example during deserialization. The
        /// initialization code is not thread-safe. This is the reason this method is not called
        /// from the static constructor of <c>HapticController</c> or <c>HapticReceiver</c>.
        public static bool Init()
        {
            if (!lofeltHapticsInitalized)
            {
                lofeltHapticsInitalized = true;

                var syncContext = System.Threading.SynchronizationContext.Current;
                playbackFinishedTimer.Elapsed += (object obj, System.Timers.ElapsedEventArgs args) =>
                {
                    // Timer elapsed events are called from a separate thread, so use
                    // SynchronizationContext to handle it in the main thread.
                    syncContext.Post(_ =>
                    {
                        HandleFinishedPlayback();
                    }, null);
                };

                if (Utils.IsVersionSupported())
                {
                    LofeltHaptics.Initialize();
                    deviceMeetsMinimumRequirements = LofeltHaptics.DeviceMeetsMinimumPlatformRequirements();
                }
            }
            return deviceMeetsMinimumRequirements;
        }

        /// <summary>
        /// Indicates if the device meets the requirements to play haptics with Nice Vibrations
        /// </summary>
        ///
        /// While <c>Utils.IsVersionSupported()</c> only checks the OS version, this method
        /// additionally checks the device capabilities.
        ///
        /// The required device capabilities are:
        /// - iOS: iPhone >= 8
        /// - Android: Amplitude control for the <c>Vibrator</c>
        ///
        /// You don't usually need to call this method. All other methods in this class will check
        /// <c>DeviceMeetsMinimumRequirements()</c> before calling into <c>LofeltHaptics</c>, and
        /// will do nothing if the minimum requirements are not met.
        public static bool DeviceMeetsMinimumRequirements()
        {
            Init();
            return deviceMeetsMinimumRequirements;
        }

        /// <summary>
        /// Loads a haptic clip given in a JSON string format for later playback
        /// </summary>
        ///
        /// At the moment only one clip can be loaded at a time.
        ///
        /// <param name="data">The haptic clip, as a JSON string of the <c>.haptic</c>
        /// file content</param>
        public static void Load(string data)
        {
            if (Init())
            {
                LofeltHaptics.Load(data);
                previousClipLoadedData = data;
                clipLoadedDuration = LofeltHaptics.GetClipDuration();
                lastSeekTime = 0.0f;
                LofeltHaptics.SetAmplitudeMultiplication(_hapticLevel);
            }
            LoadedClipChanged?.Invoke();
        }


        /// <summary>
        /// Loads the <c>HapticClip</c> given as an argument
        /// </summary>
        /// <param name="clip">the <c>HapticClip</c> to be loaded</param>>
        public static void Load(HapticClip clip)
        {
            Load(clip.GetData());
        }

        static void HandleFinishedPlayback()
        {
            lastSeekTime = 0.0f;
            isPlaybackLooping = false;
            playbackFinishedTimer.Enabled = false;
            PlaybackStopped?.Invoke();
        }

        /// <summary>
        /// Plays the haptic clip that was previously loaded with <c>Load()</c>
        /// </summary>
        ///
        /// If <c>Loop(true)</c> was called previously, the playback will be repeated
        /// until <c>Stop()</c> is called. Otherwise the haptic clip will only play once.
        public static void Play()
        {
            if (!_hapticsEnabled)
            {
                return;
            }

            if (Init())
            {
                LofeltHaptics.Play();
            }
            isPlaybackLooping = isLoopingEnabled;
            PlaybackStarted?.Invoke();

            //
            // Call HandleFinishedPlayback() after the playback finishes
            //
            float remainingPlayDuration = Mathf.Max(clipLoadedDuration - lastSeekTime, 0.0f);
            if (remainingPlayDuration > 0.0f)
            {
                playbackFinishedTimer.Interval = remainingPlayDuration * 1000;
                playbackFinishedTimer.AutoReset = false;
                playbackFinishedTimer.Enabled = !isLoopingEnabled;
            }
            else
            {
                // Setting playbackFinishedTimer.Interval needs an interval > 0, otherwise it will
                // throw an exception.
                // Even if the remaining play duration is 0, we still want to trigger everything
                // that happens in HandleFinishedPlayback().
                // A playback duration of 0 happens in the Unity editor, when loading the clip
                // failed or when seeking to the end of a clip.
                HandleFinishedPlayback();
            }
        }


        /// <summary>
        /// Loads and plays the <c>HapticClip</c> given as an argument
        /// </summary>
        /// <param name="clip">the <c>HapticClip</c> to be played</param>
        public static void Play(HapticClip clip)
        {
            Load(clip);
            Play();
        }

        /// <summary>
        /// Stops playback of the haptic clip that was previously started with <c>Play()</c>
        /// </summary>
        public static void Stop()
        {
            if (Init())
            {
                LofeltHaptics.Stop();
            }
            HandleFinishedPlayback();
        }

        /// <summary>
        /// Jumps to a time position in the haptic clip
        /// </summary>
        ///
        /// The playback will always be stopped when this function is called.
        /// This is to match the behavior between iOS and Android, since Android needs to
        /// restart playback for seek to have effect.
        ///
        /// If seeking beyond the end of the clip, play will not reproduce any haptics.
        /// Seeking to a negative position will seek to the beginning of the clip.
        ///
        /// <param name="time"> The new position within the clip, as seconds from the beginning
        /// of the clip</param>
        public static void Seek(float time)
        {
            if (Init())
            {
                LofeltHaptics.Stop();
                LofeltHaptics.Seek(time);
            }
            lastSeekTime = time;
        }

        /// <summary>
        /// Multiplies the amplitude of every breakpoint of the clip with the given multiplication
        /// factor before playing it
        /// </summary>
        ///
        /// In other words, this function applies a gain (for factors >1) or an attenuation (for
        /// factors <1) to the clip. It can be interpreted as the "volume control" for haptic
        /// playback.
        /// If the resulting amplitude of a breakpoint is greater than 1.0, it is clipped to 1.0.
        /// The amplitude is clipped hard, no limiter is used.
        ///
        /// The clip needs to be loaded with <c>Load</c> first. Loading a clip resets the
        /// multiplication factor back to the default of 1.0.
        ///
        /// On Android, the new amplitudes will take effect in the next call to <c>Play()</c>. On
        /// iOS, a call to this function will change the amplitude multiplication of a currently
        /// playing clip right away.
        ///
        /// <param name="amplitudeMultiplication"> The factor by which each amplitude will be
        /// multiplied. This value is a multiplication factor, it is <i>not</i> a dB value. The
        /// factor needs to be 0 or greater.</param>
        public static void SetAmplitudeMultiplication(float factor)
        {
            if (Init())
            {
                LofeltHaptics.SetAmplitudeMultiplication(factor);
            }
            previousAmplitudeMultiplicationValue = factor;
        }

        /// <summary>
        /// Adds the given shift to the frequency of every breakpoint in the clip, including the
        /// emphasis
        /// </summary>
        ///
        /// In other words, this function shifts all frequencies of the clip.
        /// If the resulting frequency of a breakpoint is smaller than 0.0 or greater than 1.0, it
        /// is clipped to that range. The frequency is clipped hard, no limiter is used.
        ///
        /// The clip needs to be loaded with <c>Load</c> first. Loading a clip resets the shift back
        /// to the default of 0.0.
        ///
        /// Setting the frequency shift has no effect on Android, it only works on iOS.
        ///
        /// A call to this function will change the frequency shift of a currently playing clip
        /// right away. If no clip is playing, the shift is applied in the next call to
        /// <c>Play()</c>.
        ///
        /// <param name="shift"> The amount by which each frequency should be shifted. This number
        /// is added to each frequency value. The shift needs to be between -1.0 and 1.0.</param>
        public static void SetFrequencyShift(float shift)
        {
            if (Init())
            {
                LofeltHaptics.SetFrequencyShift(shift);
            }
            previousFrequencyShift = shift;
        }

        /// <summary>
        /// Set the playback of a haptic clip to loop
        /// </summary>
        ///
        /// On Android, calling this will always put the playback position at the start of the clip.
        /// Also, it will only have an effect when <c>Play()</c> is called again.
        ///
        /// On iOS, if a clip is already playing, calling this will leave the playback position as
        /// it is and repeat when it reaches the end. No need to call <c>Play()</c> again for
        /// changes to take effect.
        ///
        /// <param name="enabled">If the value is <c>true</c>, looping will be enabled which results
        /// in repeating the playback until <c>Stop()</c> is called; if <c>false</c>, the haptic
        /// clip will only be played once.</param>
        public static void Loop(bool enabled)
        {
            if (Init())
            {
                LofeltHaptics.Loop(enabled);
            }
            isLoopingEnabled = enabled;
        }

        /// <summary>
        /// Checks if the loaded haptic clip is playing
        /// </summary>
        public static bool IsPlaying()
        {
            if (playbackFinishedTimer.Enabled)
            {
                return true;
            }
            else
            {
                return isPlaybackLooping;
            }
        }


        /// <summary>
        /// Helper that reloads haptic clip data with the set proprieties like
        /// loop and modulation
        /// </summary>
        static void Reload()
        {
            if (previousClipLoadedData != null)
            {
                Load(previousClipLoadedData);
                Loop(isLoopingEnabled);

                if (previousAmplitudeMultiplicationValue != 1.0f)
                {
                    SetAmplitudeMultiplication(previousAmplitudeMultiplicationValue);
                }

                if (previousFrequencyShift != 0.0f)
                {
                    SetFrequencyShift(previousFrequencyShift);
                }
            }
        }

        /// <summary>
        /// Stops playback and resets the playback state
        /// </summary>
        ///
        /// Seek position, amplitude multiplication, frequency shift and loop are reset to the
        /// default values.
        /// The currently loaded clip stays loaded.
        /// <c>hapticsEnabled</c> and <c>hapticLevel</c> and not reset.
        public static void Reset()
        {
            if (previousClipLoadedData != null)
            {
                Seek(0.0f);
                Stop();
                SetAmplitudeMultiplication(1.0f);
                SetFrequencyShift(1.0f);
                Loop(false);
            }
        }

        /// <summary>
        /// Processes an application focus change event
        /// </summary>
        ///
        /// If you have a <c>HapticReceiver</c> in your scene, the <c>HapticReceiver</c>
        /// will take care of calling this method when needed. Otherwise it is your
        /// responsibility to do so.
        ///
        /// When the application loses the focus, playback is stopped.
        ///
        /// On iOS, the controller is released while the application doesn't have focus,
        /// as otherwise haptic playback after the application gains focus again wouldn't
        /// work.
        ///
        /// When the application re-gains focus, it will load the previously loaded
        /// haptic clip data
        public static void ProcessApplicationFocus(bool hasFocus)
        {
            if (Init())
            {
                if (hasFocus)
                {
                    LofeltHaptics.InitializeIosController();
                    Reload();
                }
                else
                {
                    Stop();
                    LofeltHaptics.ReleaseIosController();
                }
            }
        }
    }
}
