using MoreMountains.NiceVibrations;
public static class Haptics
{
    public static void Light(){
        if(DataManager.GetVibration()) MMVibrationManager.Haptic (HapticTypes.LightImpact);
    } 
    public static void Medium(){
        if(DataManager.GetVibration()) MMVibrationManager.Haptic (HapticTypes.MediumImpact);
    } 
    public static void Heavy(){
        if(DataManager.GetVibration()) MMVibrationManager.Haptic (HapticTypes.HeavyImpact);
    } 
    public static void Soft(){
        if(DataManager.GetVibration()) MMVibrationManager.Haptic (HapticTypes.SoftImpact);
    } 
    public static void Success(){
        if(DataManager.GetVibration()) MMVibrationManager.Haptic (HapticTypes.Success);
    }    
    public static void Continuous(float intensity,float sharpnes,float duration){
        if(DataManager.GetVibration()) MMVibrationManager.ContinuousHaptic(intensity,sharpnes,duration);
    } 
}
