using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class DestroyableTilemap : MonoBehaviour, IDestroyable
{
    [SerializeField] float rayThickness = 1f;
    Tilemap tilemap;
    TilemapCollider2D myCollider;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        myCollider = GetComponent<TilemapCollider2D>();
    }

    public void OnCollide(Bullet bullet)
    {
        //add offset with the collision's normal
        Vector3 direction = bullet.Trajectory * rayThickness;
        Vector3 hitPos = myCollider.ClosestPoint(bullet.transform.position + direction);

        var coordinates = tilemap.WorldToCell(hitPos);
        var tile = tilemap.GetTile(coordinates);
        //Debug.Log(hitPos + " " + coordinates);
        if (tile)
        {
            Debug.Log($"destroy tile {tile} on coordinates : {coordinates}");
            tilemap.SetTile(coordinates, null);
            Destroy(bullet.gameObject);
        }
    }
}
