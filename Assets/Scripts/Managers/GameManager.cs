using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    public CardHand cardHand;
    public CardHighlight cardHighlight;
    public GridInput gridInput;
    public GridHighlight gridHighlight;
    public CameraController cameraController;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text resultText;

    private Card pointedCard;
    private Card draggedCard;
    private Cell pointedCell;
    private Cell selectedCell;

    public bool IsDraggingCard => draggedCard != null;
    public bool IsPlayingCard { get; private set; }
    public bool IsGameOver { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CardPlayer.Instance.Finished += OnCardFinished;
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

        if (SelectPointedCell())
        {
            TryPlayCard();
            return;
        }

        ClearDraggedCard();
    }

    private void ClearDraggedCard()
    {
        if (draggedCard == null)
            return;

        draggedCard.OnDragging(false);
        draggedCard.EndDragVisual();

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

        if (draggedCard.data.cost > TurnManager.Instance.Energy)
        {
            ClearDraggedCard();
            gridHighlight.Clear();
            selectedCell = null;
            return;
        }

        TurnManager.Instance.SpendEnergy(
            draggedCard.data.cost
        );

        IsPlayingCard = true;

        CardPlayer.Instance.Play(
            draggedCard.data,
            selectedCell,
            draggedCard
        );
    }

    private void OnCardFinished()
    {
        IsPlayingCard = false;

        if (draggedCard == null)
            return;

        Card card = draggedCard;

        ClearDraggedCard();

        CardManager.Instance.RemoveCard(card);

        gridHighlight.Clear();

        selectedCell = null;
    }

    public void EndGame(bool victory)
    {
        IsGameOver = true;

        endGamePanel.SetActive(true);

        resultText.text = victory
            ? "You win!"
            : "You lose!";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void OnDestroy()
    {
        if (CardPlayer.Instance != null)
            CardPlayer.Instance.Finished -= OnCardFinished;
    }
}
