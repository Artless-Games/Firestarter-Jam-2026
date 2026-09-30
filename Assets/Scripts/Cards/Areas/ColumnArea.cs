using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Areas/Column")]
public class ColumnArea : CardArea
{
    public override List<Cell> GetCells(Cell origin)
    {
        List<Cell> cells = new();

        int x = origin.Coordinates.x;

        for (int z = 0; z < GridManager.Instance.height; z++)
        {
            Cell cell = GridManager.Instance.GetCell(
                new Vector2Int(x, z)
            );

            if (cell != null)
                cells.Add(cell);
        }

        return cells;
    }
}
