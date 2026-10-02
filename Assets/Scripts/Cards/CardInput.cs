using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Card))]
public class CardInput : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private Card card;

    private void Awake()
    {
        card = GetComponent<Card>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.IsDraggingCard ||
            GameManager.Instance.IsPlayingCard ||
            TurnManager.Instance.IsProcessingPlayerPhase ||
            GameManager.Instance.IsGameOver)
            return;

        GameManager.Instance.PointCard(card);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Instance.IsDraggingCard ||
            GameManager.Instance.IsPlayingCard ||
            TurnManager.Instance.IsProcessingPlayerPhase ||
            GameManager.Instance.IsGameOver)
            return;

        GameManager.Instance.ClearPointedCard();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.IsPlayingCard ||
            TurnManager.Instance.IsProcessingPlayerPhase ||
            GameManager.Instance.IsGameOver)
            return;

        GameManager.Instance.BeginCardDrag(card);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.IsPlayingCard ||
            TurnManager.Instance.IsProcessingPlayerPhase ||
            GameManager.Instance.IsGameOver)
            return;

        GameManager.Instance.DragCard(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.IsPlayingCard ||
            TurnManager.Instance.IsProcessingPlayerPhase ||
            GameManager.Instance.IsGameOver)
            return;

        GameManager.Instance.EndCardDrag(eventData.position);
    }
}
