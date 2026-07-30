using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class DestroyableTilemap : MonoBehaviour, IObstacle
{
    [SerializeField] int hpPerTile = 3;
    [SerializeField] float rayThickness = 1f;
    Tilemap tilemap;
    TilemapCollider2D myCollider;
    GroundCell[,] groundCells;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        myCollider = GetComponent<TilemapCollider2D>();
    }

    void Start()
    {
        InitGroundCells();
    }

    void InitGroundCells()
    {
        groundCells = new GroundCell[tilemap.cellBounds.xMax, tilemap.cellBounds.yMax];
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int coordinates = new Vector3Int(pos.x, pos.y);
            Vector3 place = tilemap.CellToWorld(coordinates);
            if (tilemap.HasTile(coordinates))
                groundCells[coordinates.x, coordinates.y] = new GroundCell(hpPerTile, tilemap, (Vector2Int)coordinates);
        }
    }

    public void OnCollide(Bullet bullet)
    {
        //add offset with the collision's normal
        Vector3 direction = bullet.Trajectory * rayThickness;
        Vector3 hitPos = myCollider.ClosestPoint(bullet.transform.position + direction);

        var coordinates = tilemap.WorldToCell(hitPos);
        var tile = tilemap.HasTile(coordinates);
        //Debug.Log(hitPos + " " + coordinates);
        if (tile)
        {
            //tilemap.SetTile(coordinates, null);
            var cell = groundCells[coordinates.x, coordinates.y];
            cell.TakeDamage(1);
            if (cell.Health.isDead)
                groundCells[coordinates.x, coordinates.y] = null;
        }
    }
}
