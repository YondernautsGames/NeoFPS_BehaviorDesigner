using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.BehaviourDesigner
{
    public interface IAiWeapon
    {
        float damageAmount { get; }
        float kickDuration { get; }
        float kickDistance { get; }
        float kickRotation { get; }
        float recoveryTime { get; }
        float timeToImpact { get; }
    }
}
