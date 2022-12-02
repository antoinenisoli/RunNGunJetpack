using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerCapacity : PlayerModule
{
    [SerializeField] Text cooldownText;
    [SerializeField] protected float cooldown;
    bool locked;

    IEnumerator Cooldown()
    {
        float timer = 0;
        while (timer < cooldown)
        {
            yield return null;
            timer += Time.deltaTime;
            if (cooldownText)
            {
                float step = cooldown - timer;
                cooldownText.text = step.ToString("0.#");
            }
        }

        locked = false;
        if (cooldownText)
            cooldownText.text = "0";
    }

    public abstract void Effect();

    public override void Use()
    {
        base.Use();

        if (!locked)
        {
            Effect();
            locked = true;
            StartCoroutine(Cooldown());
        }
    }
}
