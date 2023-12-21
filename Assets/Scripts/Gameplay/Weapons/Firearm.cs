using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Firearm : Weapon
{
    [Header(nameof(Firearm))]
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected LayerMask blockLayer;
    [SerializeField] protected float shootDistance = 1.5f;

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = IsBlocked() ? Color.red : Color.green;
            Gizmos.DrawRay(shootPoint.position, shootPoint.right * shootDistance);
        }
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

    public virtual bool IsBlocked()
    {
        return Physics2D.Raycast(shootPoint.position, shootPoint.right, shootDistance, blockLayer);
    }
}
