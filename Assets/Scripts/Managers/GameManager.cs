using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    public CardHand cardHand;
    public CardHighlight cardHighlight;
    public GridInput gridInput;
    public GridHighlight gridHighlight;
    public CameraController cameraController;

    private Card pointedCard;
    private Card draggedCard;
    private Cell pointedCell;
    private Cell selectedCell;

    public bool IsDraggingCard => draggedCard != null;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PointCard(Card card)
    {
        if (pointedCard == card)
            return;

        pointedCard = card;

        if (pointedCard != null)
        {
            cardHand.PointCard(pointedCard);
            cardHighlight.ShowPointed(pointedCard);
        }
        else
        {
            cardHand.ClearPointedCard();
            cardHighlight.ClearPointed();
        }
    }

    public void ClearPointedCard()
    {
        pointedCard = null;

        cardHand.ClearPointedCard();
        cardHighlight.ClearPointed();
    }

    public void BeginCardDrag(Card card)
    {
        if (card == null)
            return;

        draggedCard = card;
        card.OnDragging(true);
        card.BeginDragVisual();

        cardHand.ClearPointedCard();
        cardHighlight.ClearPointed();

        cardHand.SetDraggedCard(card);

        cameraController.SetDragMode(true);
    }

    public void DragCard(Vector2 screenPosition)
    {
        if (draggedCard == null)
            return;

        draggedCard.SetDragPosition(screenPosition);

        Cell cell = gridInput.GetPointedCell(screenPosition);

        if (cell != null)
        {
            PointCell(cell);
            return;
        }

        ClearCellHighlight();
    }

    public void EndCardDrag(Vector2 screenPosition)
    {
        if (draggedCard == null)
            return;

        Card card = draggedCard;

        if (SelectPointedCell())
        {
            TryPlayCard();

            draggedCard = null;

            cameraController.SetDragMode(false);
            cardHand.ClearDraggedCard();

            CardManager.Instance.RemoveCard(card);

            return;
        }

        card.OnDragging(false);
        card.EndDragVisual();

        draggedCard = null;

        ClearCellHighlight();

        cameraController.SetDragMode(false);
        cardHand.ClearDraggedCard();
    }

    public void PointCell(Cell cell)
    {
        if (draggedCard == null)
            return;

        if (pointedCell == cell)
            return;

        pointedCell = cell;

        if (!draggedCard.data.CanTarget(cell))
        {
            gridHighlight.ShowDisallowed(cell);
            return;
        }

        List<Cell> cells =
            draggedCard.data.area.GetCells(cell);

        gridHighlight.ShowHighlight(cell, cells);
    }

    public void ClearCellHighlight()
    {
        pointedCell = null;
        gridHighlight.ClearHighlighted();
    }

    private bool SelectPointedCell()
    {
        if (draggedCard == null)
            return false;

        if (pointedCell == null)
            return false;

        if (!draggedCard.data.CanTarget(pointedCell))
            return false;

        selectedCell = pointedCell;

        List<Cell> cells =
            draggedCard.data.area.GetCells(selectedCell);

        gridHighlight.ShowSelected(cells);

        return true;
    }

    private void TryPlayCard()
    {
        if (draggedCard == null || selectedCell == null)
            return;

        if (!draggedCard.data.CanTarget(selectedCell))
            return;

        draggedCard.data.ApplyEffect(selectedCell);
    }
}
