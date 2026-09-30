using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    public GridManager gridManager;
    public GridHighlight gridHighlight;

    private Cell pointedCell;
    private Cell selectedCell;
    public Card card;

    public Card SelectedCard { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SelectedCard = card;
    }

    public void PointCell(Cell cell)
    {
        if (SelectedCard == null)
            return;

        if (pointedCell == cell)
            return;

        pointedCell = cell;

        if (!SelectedCard.data.target.CanTarget(cell))
        {
            gridHighlight.ShowDisallowed(cell);
            return;
        }

        List<Cell> cells =
            SelectedCard.data.area.GetCells(cell);

        gridHighlight.ShowHighlight(cell, cells);
    }

    public void ClearCellHighlight()
    {
        pointedCell = null;
        gridHighlight.ClearHighlighted();
    }

    public void SelectPointedCell()
    {
        if (SelectedCard == null)
            return;

        if (pointedCell == null)
            return;

        if (!SelectedCard.data.target.CanTarget(pointedCell))
            return;

        selectedCell = pointedCell;

        List<Cell> cells =
            SelectedCard.data.area.GetCells(selectedCell);

        gridHighlight.ShowSelected(cells);

        TryPlayCard();
    }

    private void TryPlayCard()
    {
        if (SelectedCard == null || selectedCell == null)
            return;

        if (!SelectedCard.data.CanTarget(selectedCell))
            return;

        SelectedCard.data.ApplyEffect(selectedCell);

        SelectedCard = null;
        pointedCell = null;
        selectedCell = null;
    }
}
