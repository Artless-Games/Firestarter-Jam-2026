using UnityEngine;

public class CardHighlight : MonoBehaviour
{
    private Card pointedCard;

    public void ShowPointed(Card card)
    {
        ClearPointed();

        pointedCard = card;

        if (pointedCard != null)
            pointedCard.OnPoint(true);
    }

    public void ClearPointed()
    {
        if (pointedCard != null)
        {
            pointedCard.OnPoint(false);
            pointedCard = null;
        }
    }

    public void Clear()
    {
        ClearPointed();
    }
}
