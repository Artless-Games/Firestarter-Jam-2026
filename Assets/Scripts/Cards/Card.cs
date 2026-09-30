using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData data;
    public float pointedHeight = 0.5f;

    private Vector3 initialPosition;
    private bool isPointed;
    private bool isSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.localPosition;
    }

    public void OnPoint(bool active)
    {
        isPointed = active;
        UpdateCard();
    }

    public void OnSelect(bool active)
    {
        isSelected = active;
        UpdateCard();
    }

    private void UpdateCard()
    {
        if (isSelected || isPointed)
        {
            Vector3 position = initialPosition;
            position.y += pointedHeight;

            transform.localPosition = position;
        }
        else
        {
            transform.localPosition = initialPosition;
        }
    }
}
