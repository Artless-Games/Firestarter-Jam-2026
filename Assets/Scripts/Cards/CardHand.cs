using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour
{
    public float spacing = 120f;
    public float arcHeight = 40f;
    public float maxRotation = 10f;

    public float pointedSpacing = 30f;
    public float draggedCardOffset = 40f;

    private readonly List<Card> cards = new();
    private Card pointedCard;
    private Card draggedCard;

    public void AddCard(Card card)
    {
        if (card == null || cards.Contains(card))
            return;

        cards.Add(card);
        ArrangeCards();
    }

    public void RemoveCard(Card card)
    {
        if (card == null)
            return;

        cards.Remove(card);

        if (pointedCard == card)
            pointedCard = null;

        if (draggedCard == card)
            draggedCard = null;

        ArrangeCards();
    }

    public void PointCard(Card card)
    {
        if (pointedCard == card)
            return;

        pointedCard = card;

        ArrangeCards();
    }

    public void ClearPointedCard()
    {
        if (pointedCard == null)
            return;

        pointedCard = null;

        ArrangeCards();
    }

    public void SetDraggedCard(Card card)
    {
        draggedCard = card;
        ArrangeCards();
    }

    public void ClearDraggedCard()
    {
        draggedCard = null;
        ArrangeCards();
    }

    private void ArrangeCards()
    {
        int count = cards.Count;

        if (count == 0)
            return;

        float center = (count - 1) * 0.5f;

        int pointedIndex = pointedCard != null
            ? cards.IndexOf(pointedCard)
            : -1;

        for (int i = 0; i < count; i++)
        {
            Card card = cards[i];

            float offset = i - center;

            float x = offset * spacing;
            float y = -Mathf.Abs(offset) * arcHeight;

            if (pointedIndex != -1)
            {
                if (i < pointedIndex)
                    x -= pointedSpacing;
                else if (i > pointedIndex)
                    x += pointedSpacing;
            }

            if (draggedCard != null && card != draggedCard)
                y -= draggedCardOffset;

            Vector2 position = new Vector2(x, y);

            float rotation = -offset * maxRotation;

            card.SetHandPosition(position, rotation);
        }
    }
}
