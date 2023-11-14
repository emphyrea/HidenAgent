using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public delegate void OnHealthChange(float currentHealth, float delta, float maxHealth);
    public delegate void OnHealthEmpty(float delta, float maxHealth);
    public delegate void OnTakenDamage(float currentHealth, float delta, float maxHealth, GameObject instigator);

    public event OnHealthChange onHealthChange;
    public event OnHealthEmpty onHealthEmpty;   
    public event OnTakenDamage onTakenDamage;

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    public void ChangeHealth(float amt, GameObject instigator)
    {
        if(amt == 0 || currentHealth == 0)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth += amt, 0, maxHealth);

        onHealthChange?.Invoke(currentHealth, amt, maxHealth);
        if(amt < 0)
        {
            onTakenDamage?.Invoke(currentHealth, amt, maxHealth, instigator);
        }
        if (currentHealth == 0)
        {
            onHealthEmpty?.Invoke(amt, maxHealth);
        }
    }
}
