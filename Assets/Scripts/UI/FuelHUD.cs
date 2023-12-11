using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelHUD : MonoBehaviour
{
    [SerializeField] Slider fuelSlider;
    [SerializeField] PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (fuelSlider && player)
            fuelSlider.value = player.GetFuelCapacity();
    }
}
