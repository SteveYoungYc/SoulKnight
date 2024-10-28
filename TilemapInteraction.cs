using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapInteraction : MonoBehaviour
{
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePosition = tilemap.WorldToCell(mouseWorldPos);
            
            if (tilemap.HasTile(tilePosition))
            {
                InteractWithTile(tilePosition);
            }
        }
    }

    void InteractWithTile(Vector3Int tilePosition)
    {
        Vector3 worldPosition = tilemap.CellToWorld(tilePosition);
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Turret");
        GameObject bulletGameObject = Instantiate(prefab);
        bulletGameObject.transform.position = worldPosition + tilemap.tileAnchor / 2;
    }
}
