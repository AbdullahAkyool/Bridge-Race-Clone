using UnityEngine;
using System.IO;
using UnityEngine.Bindings;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#elif UNITY_2019_4_OR_NEWER
using UnityEditor.Experimental.AssetImporters;
#endif

namespace Lofelt.NiceVibrations
{
    [ScriptedImporter(version: 1, ext: "haptic", AllowCaching = true)]
    /// <summary>
    /// Provides an importer for the <c>HapticClip</c> component.
    /// </summary>
    ///
    /// The importer takes a .haptic file and converts it into a <c>HapticClip</c>.
        public class HapticImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var hapticClip = HapticClip.CreateInstance<HapticClip>();
            var fileName = System.IO.Path.GetFileNameWithoutExtension(ctx.assetPath);
            hapticClip.SetData(File.ReadAllText(ctx.assetPath));
            ctx.AddObjectToAsset("com.lofelt.HapticClip", hapticClip);
            ctx.SetMainObject(hapticClip);
        }
    }
}
