using System.Collections.Generic;
using UnityEngine;

public class GridHighlight : MonoBehaviour
{
    private Cell pointedCell;

    private readonly List<Cell> highlightedCells = new();
    private readonly List<Cell> selectedCells = new();

    public void ShowHighlight(Cell pointedCell, List<Cell> cells)
    {
        ClearHighlighted();

        this.pointedCell = pointedCell;

        foreach (Cell cell in cells)
        {
            if (cell != null)
            {
                cell.OnHighlight(true);
                highlightedCells.Add(cell);
            }
        }

        if (pointedCell != null)
            pointedCell.OnPoint(true);
    }

    public void ShowDisallowed(Cell cell)
    {
        ClearHighlighted();

        pointedCell = cell;

        if (pointedCell != null)
            pointedCell.OnDisallowed(true);
    }

    public void ShowSelected(List<Cell> cells)
    {
        ClearHighlighted();

        ClearSelected();

        foreach (Cell cell in cells)
        {
            if (cell != null)
            {
                cell.OnSelect(true);
                selectedCells.Add(cell);
            }
        }
    }

    public void ClearHighlighted()
    {
        foreach (Cell cell in highlightedCells)
        {
            if (cell != null)
                cell.OnHighlight(false);
        }

        highlightedCells.Clear();

        if (pointedCell != null)
        {
            pointedCell.OnPoint(false);
            pointedCell.OnDisallowed(false);
            pointedCell = null;
        }
    }

    public void ClearSelected()
    {
        foreach (Cell cell in selectedCells)
        {
            if (cell != null)
                cell.OnSelect(false);
        }

        selectedCells.Clear();
    }

    public void Clear()
    {
        ClearHighlighted();
        ClearSelected();
    }
}
