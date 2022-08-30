using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoSingleton<Finish>, IInterractable
{
    public IMultiplier multiplier;

    public void Interract(Transform interractor){
        Player.Instance.cc.Disable();
        
        ParticleManager.Instance.Launch(0,transform.position + Vector3.up,2f);

    }
}
