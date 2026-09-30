using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Areas/Row")]
public class RowArea : CardArea
{
    public override List<Cell> GetCells(Cell origin)
    {
        List<Cell> cells = new();

        int z = origin.Coordinates.y;

        for (int x = 0; x < GridManager.Instance.width; x++)
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
