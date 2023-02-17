using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Entity entity;

    private void Update()
    {
        if (healthSlider && entity)
            healthSlider.value = entity.Health.value;
    }
}
