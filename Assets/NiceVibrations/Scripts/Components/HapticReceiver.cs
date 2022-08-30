using UnityEngine;

namespace Lofelt.NiceVibrations
{
    /// <summary>
    /// A <c>MonoBehaviour</c> that forwards global properties from <c>HapticController</c> and
    /// handles events
    /// </summary>
    ///
    /// While <c>HapticSource</c> provides a per-clip <c>MonoBehaviour</c> API for the functionality
    /// in <c>HapticController</c>, <c>HapticReceiver</c> provides a <c>MonoBehaviour</c> API for
    /// the global functionality in <c>HapticController</c>.
    ///
    /// <c>HapticReceiver</c> is also responsible for global event handling, such as an application
    /// focus change. To make this work correctly, your scene should have exactly one
    /// <c>HapticReceiver</c> component, similar to how a scene should have exactly one
    /// <c>AudioListener</c>.
    ///
    /// In the future <c>HapticReceiver</c> might receive parameters and distance to
    /// <c>HapticSource</c> components, and can be used for global parameter control through Unity
    /// Editor GUI.
    [AddComponentMenu("Nice Vibrations/Haptic Receiver")]
    public class HapticReceiver : MonoBehaviour, ISerializationCallbackReceiver
    {
        // These two fields are only used for serialization and deserialization.
        // HapticController manages the global haptic level and global haptic toggle,
        // HapticReceiver forwards these properties so they are available in a
        // MonoBehaviour.
        // To be able to serialize these properties, HapticReceiver needs to have
        // fields for them. Before serialization, these fields are set to the values
        // from HapticController, and after deserialization the values are restored
        // back to HapticController.
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float _hapticLevel = 1.0f;
        [SerializeField]
        private bool _hapticsEnabled = true;

        public void OnBeforeSerialize()
        {
            _hapticLevel = HapticController._hapticLevel;
            _hapticsEnabled = HapticController._hapticsEnabled;
        }

        public void OnAfterDeserialize()
        {
            HapticController._hapticLevel = _hapticLevel;
            HapticController._hapticsEnabled = _hapticsEnabled;
        }

        /// <summary>
        /// Forwarded <c>HapticController::hapticLevel</c>
        /// </summary>
        [System.ComponentModel.DefaultValue(1.0f)]
        public float hapticLevel
        {
            get { return HapticController.hapticLevel; }
            set { HapticController.hapticLevel = value; }
        }


        /// <summary>
        /// Forwarded <c>HapticController::hapticsEnabled</c>
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        public bool hapticsEnabled
        {
            get { return HapticController.hapticsEnabled; }
            set { HapticController.hapticsEnabled = value; }
        }

        /// <summary>
        /// Initializes <c>HapticController</c> so that the initialization time is spent at
        /// startup instead of when the first haptic is triggered during gameplay
        /// </summary>
        void Start()
        {
            HapticController.Init();
        }

        /// <summary>
        /// Forwards an application focus change event to <c>HapticController</c>
        /// </summary>
        void OnApplicationFocus(bool hasFocus)
        {
            HapticController.ProcessApplicationFocus(hasFocus);
        }
    }
}
