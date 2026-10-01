using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    // Singleton
    public static GridManager Instance { get; private set; }

    public Terrain terrain;
    public Cell cellPrefab;

    public int width = 5;
    public int height = 5;
    public float cellSize = 5f;

    private Grid grid;
    private Cell[,] cells;

    public int BuildableCellCount
    {
        get
        {
            int count = 0;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    Cell cell = cells[x, z];

                    if (cell != null && cell.IsBuildable)
                        count++;
                }
            }

            return count;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        grid = GetComponent<Grid>();
        grid.cellSize = new Vector3(cellSize, cellSize, grid.cellSize.z);

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
            cell.Coordinates.y,
            0
        );

        Vector3 worldPosition = grid.GetCellCenterWorld(cellPosition);
        worldPosition.y = cell.TerrainHeight;

        return worldPosition;
    }

    public Cell GetCell(Vector2Int coordinates)
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
                    transform
                );

                cell.Initialize(cellSize, new Vector2Int(x, z), terrain, true, false);

                cells[x, z] = cell;
            }
        }
    }
}
