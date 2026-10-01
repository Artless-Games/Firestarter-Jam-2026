using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    public Card cardPrefab;
    public CardHand cardHand;
    public List<CardData> startingCards;

    private readonly List<Card> cards = new();

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
        CreateStartingHand();
    }

    private void CreateStartingHand()
    {
        foreach (CardData data in startingCards)
        {
            CreateCard(data);
        }
    }

    private void CreateCard(CardData data)
    {
        if (data == null)
            return;

        Card card = Instantiate(cardPrefab, cardHand.transform);

        card.data = data;

        cards.Add(card);
        cardHand.AddCard(card);
    }

    public void RemoveCard(Card card)
    {
        if (card == null)
            return;

        cards.Remove(card);
        cardHand.RemoveCard(card);

        Destroy(card.gameObject);
    }
}
