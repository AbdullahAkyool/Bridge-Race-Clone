using System;
using UnityEngine;

namespace Lofelt.NiceVibrations
{
    /// <summary>
    /// A collection of methods to play simple haptic patterns
    /// </summary>
    ///
    /// Each of the methods here load and play a simple haptic clip. After playback
    /// has finished, the clip will remain loaded in <c>HapticController</c>.
    public static class HapticPatterns
    {
        static String emphasisTemplate;
        static String constantTemplate;

        static HapticPatterns()
        {
            emphasisTemplate = (Resources.Load("nv-emphasis-template") as TextAsset).text;
            constantTemplate = (Resources.Load("nv-constant-template") as TextAsset).text;
        }

        /// <summary>
        /// Plays a single emphasis point
        /// </summary>
        ///
        /// Plays a haptic clip that consists only of one breakpoint with emphasis.
        /// On iOS, this translates to a transient, and on Android to a quick vibration.
        /// <param name="amplitude">The amplitude of the emphasis, from 0.0 to 1.0</param>
        /// <param name="frequency">The frequency of the emphasis, from 0.0 to 1.0</param>
        public static void PlayEmphasis(float amplitude, float frequency)
        {
            if (emphasisTemplate == null)
            {
                return;
            }

            float clampedAmplitude = Mathf.Clamp(amplitude, 0.0f, 1.0f);
            float clampedFrequency = Mathf.Clamp(frequency, 0.0f, 1.0f);
            const float duration = 0.1f;
            String clip = emphasisTemplate
                .Replace("{amplitude}", clampedAmplitude.ToString())
                .Replace("{frequency}", clampedFrequency.ToString())
                .Replace("{duration}", duration.ToString());
            HapticController.Load(clip);
            HapticController.Loop(false);
            HapticController.Play();
        }

        /// <summary>
        /// Plays a haptic with constant amplitude and frequency
        /// </summary>
        ///
        /// On iOS, you can use <c>HapticController::SetAmplitudeMultiplication()</c> and
        /// <c>HapticController::SetFrequencyShift()</c> to modulate the haptic while it's
        /// playing.
        ///
        /// <param name="amplitude">Amplitude, from 0.0 to 1.0</param>
        /// <param name="frequency">Frequency, from 0.0 to 1.0</param>
        /// <param name="duration">Play duration in seconds</param>
        public static void PlayConstant(float amplitude, float frequency, float duration)
        {
            if (constantTemplate == null)
            {
                return;
            }

            float clampedAmplitude = Mathf.Clamp(amplitude, 0.0f, 1.0f);
            float clampedFrequency = Mathf.Clamp(frequency, 0.0f, 1.0f);
            float clampedDuration = Mathf.Max(duration, 0.0f);
            String clip = constantTemplate
                .Replace("{duration}", clampedDuration.ToString());
            HapticController.Load(clip);
            HapticController.Loop(false);
            HapticController.SetAmplitudeMultiplication(clampedAmplitude);
            HapticController.SetFrequencyShift(clampedFrequency);
            HapticController.Play();
        }
    }
}
