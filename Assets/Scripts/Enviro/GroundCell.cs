using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundCell : IDestroyable
{
    public Health Health => health;

    public GameObject spawnObj;
    Health health;
    Tilemap tilemap;
    Vector2Int coordinates;

    public GroundCell(int maxHP, Tilemap tilemap, Vector2Int coordinates)
    {
        health = new Health();
        health.MaxHealth = maxHP;
        health.Initialize();

        this.tilemap = tilemap;
        this.coordinates = coordinates;
    }

    public void Death()
    {
        Debug.Log($"destroy tile #{coordinates} and remove it from tilemap", tilemap);
        tilemap.SetTile((Vector3Int)coordinates, null);
    }

    public void TakeDamage(int amount)
    {
        if (!health.Immortal)
        {
            Debug.Log($"tile #{coordinates} received damage ! {health.CurrentHealth}");
            health.TakeDamage(amount);
        }

        if (health.isDead)
            Death();
    }
}
