using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Areas/All")]
public class AllArea : CardArea
{
    public override List<Cell> GetCells(Cell origin)
    {
        List<Cell> cells = new();

        for (int x = 0; x < GridManager.Instance.width; x++)
        {
            for (int z = 0; z < GridManager.Instance.height; z++)
            {
                Cell cell = GridManager.Instance.GetCell(
                    new Vector2Int(x, z)
                );

                if (cell != null)
                    cells.Add(cell);
            }
        }

        return cells;
    }
}
