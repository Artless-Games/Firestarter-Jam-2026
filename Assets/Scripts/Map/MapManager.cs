using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Terrain terrain;
    public Grid grid;
    public Cell cellPrefab;

    public int width = 5;
    public int height = 5;
    public float cellSize = 5f;

    private Cell[,] cells;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid();
    }

    public Cell WorldToCell(Vector3 worldPosition)
    {
        Vector3Int cellPosition = grid.WorldToCell(worldPosition);
        Vector2Int coordinates = new Vector2Int(cellPosition.x, cellPosition.y);

        return GetCell(coordinates);
    }

    public Vector3 CellToWorld(Cell cell)
    {
        Vector3Int cellPosition = new Vector3Int(
            cell.Coordinates.x,
            0,
            cell.Coordinates.y
        );

        Vector3 worldPosition = grid.GetCellCenterWorld(cellPosition);
        worldPosition.y = terrain.SampleHeight(worldPosition)
            + terrain.transform.position.y;

        return worldPosition;
    }

    private Cell GetCell(Vector2Int coordinates)
    {
        if (!IsInsideGrid(coordinates))
            return null;

        return cells[coordinates.x, coordinates.y];
    }

    private bool IsInsideGrid(Vector2Int coordinates)
    {
        return coordinates.x >= 0 &&
               coordinates.x < width &&
               coordinates.y >= 0 &&
               coordinates.y < height;
    }

    private void GenerateGrid()
    {
        cells = new Cell[width, height];

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

                cells[x, z] = cell;
            }
        }
    }
}
