using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Areas/Square")]
public class SquareArea : CardArea
{
    public int size = 3;

    public override List<Cell> GetCells(Cell origin)
    {
        List<Cell> cells = new();

        int half = size / 2;
        Vector2Int center = origin.Coordinates;

        for (int x = -half; x <= half; x++)
        {
            for (int z = -half; z <= half; z++)
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
        size = Mathf.Max(1, size);

        if (size % 2 == 0)
            size++;
    }
}
