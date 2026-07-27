using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyableTiles : MonoBehaviour, IDestroyable
{
    [SerializeField] Tilemap tilemap;

    public void OnCollide(Bullet bullet)
    {
        Vector3 hitPos = tilemap.GetComponent<TilemapCollider2D>().ClosestPoint(bullet.transform.position);
        //add offset with the collision's normal
        var coordinates = tilemap.WorldToCell(hitPos);
        var tile = tilemap.GetTile(coordinates);
        Debug.Log(hitPos + " " + coordinates);
        Debug.Log(tile, tile);
        if (tile)
        {
            tilemap.SetTile(coordinates, null);
            Destroy(bullet.gameObject);
        }
    }
}
