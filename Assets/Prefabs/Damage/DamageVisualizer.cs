using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{
    [SerializeField] GameObject visual;
    [SerializeField] Color defaultColor;
    [SerializeField] Color damagedColor;
    Color currentColor;
    public float lerpSpeed = 10;
    [SerializeField] string colorAttributeName = "_Color";
    [SerializeField] Shaker shaker;

    List<Material> materials = new List<Material>();

    private void Awake()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = new Material(renderer.material);
            materials.Add(renderer.material);
            currentColor = renderer.material.GetColor(colorAttributeName);
        }
        currentColor = defaultColor;

        HealthComponent healthComponent = GetComponent<HealthComponent>();
        healthComponent.onTakenDamage += TookDamage;
    }

    private void TookDamage(float currentHealth, float delta, float maxHealth, GameObject instigator)
    {
        if(Mathf.Abs((currentColor - defaultColor).grayscale) < 0.1f)
        {
            currentColor = damagedColor;
            SetMaterialColor(currentColor);
        }
        if(shaker)
        {
            shaker.StartShake();
        }
    }
    private void Update()
    {
        currentColor = Color.Lerp(currentColor, defaultColor, Time.deltaTime * lerpSpeed);
        SetMaterialColor(currentColor);

    }

    void SetMaterialColor(Color color)
    {
        foreach (Material material in materials)
        {
            material.SetColor(colorAttributeName, color);
        }
    }
}
