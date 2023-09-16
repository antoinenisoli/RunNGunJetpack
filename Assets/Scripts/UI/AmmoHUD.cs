using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoHUD : MonoBehaviour
{
    [SerializeField] Text text;
    AmmoSystem ammoSystem;

    private void Awake()
    {
        ammoSystem = FindObjectOfType<AmmoSystem>();
    }

    private void Update()
    {
        if (!ammoSystem)
            return;

        text.text = ammoSystem.AmmoAmount + "";
    }
}
