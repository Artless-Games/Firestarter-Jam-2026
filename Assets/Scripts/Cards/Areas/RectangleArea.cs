using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Areas/Rectangle")]
public class RectangleArea : CardArea
{
    public int width = 3;
    public int height = 5;

    public override List<Cell> GetCells(Cell origin)
    {
        List<Cell> cells = new();

        int minX = -(width / 2);
        int maxX = (width - 1) / 2;

        int minZ = -(height / 2);
        int maxZ = (height - 1) / 2;

        Vector2Int center = origin.Coordinates;

        for (int x = minX; x <= maxX; x++)
        {
            for (int z = minZ; z <= maxZ; z++)
            {
                Vector2Int coordinates =
                    center + new Vector2Int(x, z);

                Cell cell = GridManager.Instance.GetCell(coordinates);

                if (cell != null)
                    cells.Add(cell);
            }
        }

        return cells;
    }

    private void OnValidate()
    {
        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);
    }
}