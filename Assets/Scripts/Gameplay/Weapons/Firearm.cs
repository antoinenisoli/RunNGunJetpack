using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Firearm : Weapon
{
    [Header(nameof(Firearm))]
    [SerializeField] protected Transform shootPoint;
    [SerializeField] LayerMask blockLayer;
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

    public bool IsBlocked()
    {
        var hit = Physics2D.Linecast(shootPoint.position, shootPoint.position + shootPoint.right * shootDistance);
        if (hit && GameDevHelper.LayerMaskContains(blockLayer, hit.transform.gameObject.layer))
        {
            //print(hit.transform);
            return true;
        }
            
        return false;
    }
}
