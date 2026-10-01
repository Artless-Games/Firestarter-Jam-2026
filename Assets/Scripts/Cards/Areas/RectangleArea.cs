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

        Vector2Int start = origin.Coordinates;

        if (height > width)
        {
            start.y -= height / 2;
        }
        else
        {
            start.x -= width / 2;
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector2Int coordinates =
                    start + new Vector2Int(x, z);

                Cell cell =
                    GridManager.Instance.GetCell(coordinates);

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
