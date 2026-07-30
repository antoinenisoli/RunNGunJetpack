using UnityEngine;

public interface IDestroyable
{
    public Health Health { get; }
    public void Death();
    public void TakeDamage(int dmg);
}
