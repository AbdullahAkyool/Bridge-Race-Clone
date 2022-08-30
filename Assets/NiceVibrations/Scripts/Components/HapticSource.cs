using UnityEngine;

namespace Lofelt.NiceVibrations
{
    /// <summary>
    /// Provides haptic playback functionality for a single haptic clip
    /// </summary>
    ///
    /// <c>HapticSource</c> plays back the <c>HapticClip</c> assigned in the <c>clip</c> property
    /// when calling <c>Play()</c>. It also provides various ways to control playback, such as
    /// seeking, looping and amplitude/frequency modulation.
    ///
    /// At the moment, playback of a haptic source is not triggered automatically
    /// by e.g. proximity between the <c>HapticReceiver</c> and the <c>HapticSource</c>,
    /// so you need to call <c>Play()</c> to trigger playback.
    ///
    /// You can place multiple <c>HapticSource</c> components in your scene, with a different
    /// <c>HapticClip</c> assigned to each.
    ///
    /// <c>HapticSource</c> provides a per-clip <c>MonoBehaviour</c> API for the functionality
    /// in <c>HapticController</c>, while <c>HapticReceiver</c> provides a <c>MonoBehaviour</c> API
    /// for the global functionality in <c>HapticController</c>.
    ///
    /// <c>HapticSourceInspector</c> provides a custom editor for <c>HapticSource</c> for the
    /// Inspector.
    [AddComponentMenu("Nice Vibrations/Haptic Source")]
    public class HapticSource : MonoBehaviour
    {
        const int DEFAULT_PRIORITY = 128;

        /// The <c>HapticClip this <c>HapticSource</c> loads and plays.
        public HapticClip clip;

        /// <summary>
        /// The priority of the <c>HapticSource</c>
        /// </summary>
        ///
        /// This property is set by <c>HapticSourceInspector</c>. 0 is the highest priority and 256
        /// is the lowest priority.
        public int priority = DEFAULT_PRIORITY;

        /// <summary>
        /// Jump in time position of haptic source playback
        /// </summary>
        ///
        /// Initially set to 0.0 seconds.
        /// This value can only be set when using <c>Seek()</c>
        float seekTime = 0.0f;

        [SerializeField]
        bool _loop = false;

        /// <summary>
        /// Set the haptic source to loop playback of the haptic clip
        /// </summary>
        ///
        /// It will only have any effect once <c>Play()</c> is called.
        ///
        /// See <c>HapticController::Loop()</c> for further details.
        [System.ComponentModel.DefaultValue(false)]
        public bool loop
        {
            get { return _loop; }
            set { _loop = value; }
        }

        [SerializeField]
        float _amplitudeMultiplication = 1.0f;

        /// <summary>
        /// This multiplication factor is applied to the amplitude value of every breakpoint of the
        /// clip
        /// </summary>
        ///
        /// In addition to the multiplication factor here, the global multiplication factor set
        /// in <c>HapticController::hapticLevel</c> or <c>HapticReceiver::hapticLevel</c> is also
        /// taken into account. The final multiplication factor is a combination of both.
        ///
        /// See <c>HapticController::SetAmplitudeMultiplication()</c> for further details.
        [System.ComponentModel.DefaultValue(1.0)]
        public float amplitudeMultiplication
        {
            get { return _amplitudeMultiplication; }
            set
            {
                _amplitudeMultiplication = value;

                if (IsLoaded())
                {
                    HapticController.SetAmplitudeMultiplication(_amplitudeMultiplication * HapticController.hapticLevel);
                }
            }
        }

        [SerializeField]
        float _frequencyShift = 0.0f;

        /// <summary>
        /// This shift is added to the frequency of every breakpoint in the clip, including the
        /// emphasis
        /// </summary>
        ///
        /// See <c>HapticController::SetFrequencyShift()</c> for further details.
        [System.ComponentModel.DefaultValue(0.0)]
        public float frequencyShift
        {
            get { return _frequencyShift; }
            set
            {
                _frequencyShift = value;

                if (IsLoaded())
                {
                    HapticController.SetFrequencyShift(_frequencyShift);
                }
            }
        }

        /// The HapticSource that is currently loaded into HapticController.
        /// This can be null if nothing was ever loaded, or if HapticController::Load()
        /// was called directly, bypassing HapticSource.
        static HapticSource loadedHapticSource = null;

        /// The HapticSource that was last played.
        /// This can be null if nothing was ever player, or if HapticController::Play()
        /// was called directly, bypassing HapticSource.
        /// The lastPlayedHapticSource isn't necessarily playing now, lastPlayedHapticSource
        /// will remain set even if playback has finished or was stopped.
        static HapticSource lastPlayedHapticSource = null;

        static HapticSource()
        {
            // When HapticController::Load() or HapticController::Play() is
            // called directly, bypassing HapticSource, reset loadedHapticSource
            // and lastPlayedHapticSource.
            HapticController.LoadedClipChanged += () =>
            {
                loadedHapticSource = null;
            };
            HapticController.PlaybackStarted += () =>
            {
                lastPlayedHapticSource = null;
            };

            // When the haptic level changes, HapticController applies the new haptic level as
            // the global amplitude multiplication, without taking into account the amplitude
            // multiplication of the current HapticSource.
            // Here we subscribe to the HapticLevelChanged Action to be able to override the
            // global amplitude multiplication to take the HapticSource's amplitude multiplication
            // into account.
            HapticController.HapticLevelChanged += () =>
            {
                if (loadedHapticSource != null)
                {
                    HapticController.SetAmplitudeMultiplication(
                        loadedHapticSource.amplitudeMultiplication * HapticController.hapticLevel);
                }
            };
        }

        /// <summary>
        /// Loads and plays back the haptic clip
        /// </summary>
        ///
        /// At the moment only one haptic clip at a time can be played. If another
        /// <c>HapticSource</c> is currently playing and has lower priority, its playback will
        /// be stopped.
        ///
        /// If a <c>seekTime</c> within the time range of the clip has been set, it will jump to
        /// that position if <c>loop</c> is <c>false</c>. If <c>loop</c> is <c>true</c>, seeking
        /// will have no effect.
        ///
        /// It will loop playback in case <c>loop</c> is <c>true</c>.
        public void Play()
        {
            if (CanPlay())
            {
                //
                // Load
                //
                HapticController.Load(clip.GetData());
                loadedHapticSource = this;

                //
                // Apply properties like loop, seek position and modulation
                //
                HapticController.Loop(loop);

                if (seekTime != 0.0f && !loop)
                {
                    HapticController.Seek(seekTime);
                }

                float finalAmplitudeMultiplication = amplitudeMultiplication * HapticController.hapticLevel;
                if (finalAmplitudeMultiplication != 1.0f)
                {
                    HapticController.SetAmplitudeMultiplication(finalAmplitudeMultiplication);
                }

                if (frequencyShift != 0.0f)
                {
                    HapticController.SetFrequencyShift(frequencyShift);
                }

                //
                // Play
                //
                HapticController.Play();
                lastPlayedHapticSource = this;
            }
        }

        private bool CanPlay()
        {
            return (!HapticController.IsPlaying() ||
                   (lastPlayedHapticSource != null && priority <= lastPlayedHapticSource.priority));
        }

        /// <summary>
        /// Checks if the current <c>HapticSource</c> has been loaded into <c>HapticController</c>
        /// </summary>
        ///
        /// This is used to avoid triggering operations on <c>HapticController</c> while
        /// another <c>HapticSource</c> is loaded.
        private bool IsLoaded()
        {
            return Object.ReferenceEquals(this, loadedHapticSource);
        }

        /// <summary>
        /// Stops playback that was previously started with <c>Start()</c>
        /// </summary>
        public void Stop()
        {
            if (IsLoaded())
            {
                HapticController.Stop();
            }
        }

        /// <summary>
        /// Sets the time position to jump to when <c>Play()</c> is called
        /// </summary>
        ///
        /// It will only have an effect once <c>Play()</c> is called.
        public void Seek(float time)
        {
            this.seekTime = time;
        }

        /// <summary>
        /// When a GameObject is disabled, stop playback if this HapticSource is
        /// playing
        /// </summary>
        public void OnDisable()
        {
            if (HapticController.IsPlaying() && IsLoaded())
            {
                this.Stop();
            }
        }
    }
}
