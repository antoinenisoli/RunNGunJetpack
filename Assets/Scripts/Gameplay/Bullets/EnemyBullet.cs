using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    [Header(nameof(EnemyBullet))]
    [SerializeField] protected int damageAmount = 1;
    [SerializeField] private float speedAmount = 25;

    public override int Damage { get => damageAmount; set => base.Damage = value; }
    public override float Speed { get => speedAmount; set => base.Speed = value; }

    public override bool CantTakeDamage(Entity entity)
    {
        return entity is Enemy;
    }

    void LookAtTrajectory()
    {
        float angle = Mathf.Atan2(trajectory.y, trajectory.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public override void Update()
    {
        base.Update();
        LookAtTrajectory();
    }
}
