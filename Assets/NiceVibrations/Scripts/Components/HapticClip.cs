using UnityEngine;
using System;

namespace Lofelt.NiceVibrations
{
    /// <summary>
    /// Contains the data of a haptic clip.
    /// </summary>
    ///
    /// When a <c>HapticClip</c> is assigned to <c>HapticSource.clip</c>, <c>HapticSource</c>
    /// can play it.
    public class HapticClip : ScriptableObject
    {
        /// The haptic data.
        [SerializeField]
        private string data;

        public void SetData(string data)
        {
            this.data = data;
        }

        public string GetData()
        {
            return this.data;
        }
    }
}
