using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Terrain terrain;
    public Grid grid;
    public Cell cellPrefab;

    public int width = 5;
    public int height = 5;
    public float cellSize = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid();
    }

    public Vector3 GetCellCenter(Vector2Int cell)
    {
        Vector3Int gridPosition = new Vector3Int(
            cell.x,
            0,
            cell.y
        );

        return grid.GetCellCenterWorld(gridPosition);
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 position = new Vector3(
                    x * cellSize + cellSize / 2f,
                    0f,
                    z * cellSize + cellSize / 2f
                );

                Cell cell = Instantiate(
                    cellPrefab,
                    position,
                    Quaternion.identity,
                    grid.transform
                );

                cell.Initialize(cellSize, terrain, new Vector2Int(x, z));
            }
        }
    }
}
