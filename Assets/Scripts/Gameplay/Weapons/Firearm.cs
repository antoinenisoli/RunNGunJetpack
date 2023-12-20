using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Firearm : Weapon
{
    [Header(nameof(Firearm))]
    [SerializeField] protected Transform shootPoint;
    [SerializeField] LayerMask blockLayer;
    [SerializeField] float shootDistance = 1.5f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsBlocked() ? Color.red : Color.green;
        Gizmos.DrawRay(transform.position, transform.right * shootDistance);
    }

    public void PlayVFX(string fxName)
    {
        VFXManager.Instance.PlayVFX(fxName, shootPoint.position);
    }

    public virtual bool Shoot(bool useAmmo = true)
    {
        OnAttack.Invoke();
        return true;
    }

    public bool IsBlocked()
    {
        var block = Physics2D.Raycast(transform.position, transform.right, shootDistance, blockLayer);
        return block;
    }
}
