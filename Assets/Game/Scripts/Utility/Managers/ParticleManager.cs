using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoSingleton<ParticleManager>
{
    
    [SerializeField] private List<GameObject> particleEffects;
    
    /// <summary> Launches a referenced particle effect which is given to the particle manager instance (By Name) </summary>
    public void Launch(string name,Vector3 pos, float scaleMultiplier = 1f)
    {
        foreach (GameObject effect in particleEffects)
        {
            if(effect.name.Equals(name))
            {
                LaunchEffect(effect,pos,scaleMultiplier);
                return;
            }
        }
        Debug.Log("Effect:" + name + " is not in the list!");
    }

    /// <summary> Launches a referenced particle effect which is given to the particle manager instance (By Index) </summary>
    public void Launch(int index, Vector3 pos, float scaleMultiplier = 1f)
    {
        try
        {
            LaunchEffect(particleEffects[index], pos, scaleMultiplier);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.Log("Particle Manager: Effect index is out of range");
        }
        
    }
    
    private void LaunchEffect(GameObject effect,Vector3 pos, float scaleMultiplier = 1f)
    {
        GameObject go = Instantiate(effect, pos, Quaternion.identity, transform);
        go.transform.localScale *= scaleMultiplier;
    }
}
