using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perception/HitSense")]
public class HitSense : Sense
{
    GameObject damageInstigator;
    public override void Init(MonoBehaviour owner)
    {
        base.Init(owner);
       HealthComponent healthComponent = owner.GetComponent<HealthComponent>();
        if(healthComponent != null)
        {
            healthComponent.onTakenDamage += OwnerTookDamage;
        }
    }

    private void OwnerTookDamage(float currentHealth, float delta, float maxHealth, GameObject instigator)
    {
        damageInstigator = instigator;
    }

    public override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        if(stimuli.gameObject == damageInstigator)
        {
            damageInstigator = null;
            return true;
        }
        return false;
    }


}
