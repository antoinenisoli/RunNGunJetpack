using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header(nameof(MeleeWeapon))]
    [SerializeField] LayerMask targetMask;
    [SerializeField] float attackDelay;
    MeleeWeaponData meleeWeaponData;
    float timer;

    public MeleeWeaponData MeleeWeaponData
    {
        get => meleeWeaponData;
        set
        {
            meleeWeaponData = value;
            WeaponData = value;
        }
    }

    public override void GetWeaponData()
    {
        if (weaponDataObj)
        {
            var obj = Instantiate(weaponDataObj);
            MeleeWeaponData = obj.GetWeaponData();
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackDelay);
        var hits = Physics2D.OverlapCircleAll(weaponVisual.position, MeleeWeaponData.Range, targetMask);
        foreach (var item in hits)
        {
            Entity entity = item.GetComponentInChildren<Entity>();
            if (entity)
            {
                if (entity == owner)
                    continue;

                entity.TakeDamage(MeleeWeaponData.Damages);
            }
        }
    }

    public virtual void WeaponAttack()
    {
        OnAttack.Invoke();
        StartCoroutine(Attack());
    }

    public virtual void ExecuteTimer()
    {
        timer += Time.deltaTime;
        if (timer > MeleeWeaponData.AttackRate)
        {
            timer = 0;
            WeaponAttack();
        }
    }
}
