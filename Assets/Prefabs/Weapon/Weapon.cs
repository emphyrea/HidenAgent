using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] string slotName = "DefaultWeaponSlot";
    [SerializeField] AnimatorOverrideController animOverride;

    public string GetSlotName()
    {
        return slotName;
    }

    public GameObject Owner
    {
        get; private set;
    }

    public void Initialize(GameObject owner)
    {
        Owner = owner;
    }

    public void Equip()
    {
        gameObject.SetActive(true);
        Owner.GetComponent<Animator>().runtimeAnimatorController = animOverride; //applies override to animator
    }

    public void UnEquip()
    {
        gameObject.SetActive(false);
    }

    public abstract void Attack();

    protected void DamageGameObject(GameObject target, float damageAmt)
    {
        target.GetComponent<HealthComponent>()?.ChangeHealth(-damageAmt, Owner);
    }
}
