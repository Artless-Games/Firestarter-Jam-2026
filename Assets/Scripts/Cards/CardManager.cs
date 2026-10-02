using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    public Card cardPrefab;
    public CardHand cardHand;
    public List<CardData> cardPool;
    [SerializeField] private int handSize = 5;
    private List<CardData> deck = new();

    private readonly List<Card> handCards = new();
    public int DeckCount => deck.Count;

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
        CreateDeck();
    }

    public void CreateDeck()
    {
        deck = new List<CardData>(cardPool);

        ShuffleDeck();
    }

    private void ShuffleDeck()
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }

    public void DrawUpToHandSize()
    {
        int maxCardCost = TurnManager.Instance.MaxCardCost;

        while (handCards.Count < handSize)
        {
            CardData cardData = DrawCard(maxCardCost);

            if (cardData == null)
                break;

            CreateCard(cardData);
        }
    }

    private CardData DrawCard(int maxCardCost)
    {
        List<CardData> availableCards = new();

        foreach (CardData card in deck)
        {
            if (card.cost <= maxCardCost)
                availableCards.Add(card);
        }

        if (availableCards.Count == 0)
            return null;

        CardData selectedCard =
            availableCards[Random.Range(0, availableCards.Count)];

        deck.Remove(selectedCard);

        return selectedCard;
    }

    private void CreateCard(CardData data)
    {
        Card card = Instantiate(
            cardPrefab,
            cardHand.transform
        );

        card.data = data;
        card.UpdateCard();

        handCards.Add(card);
        cardHand.AddCard(card);
    }

    public void RemoveCard(Card card)
    {
        if (card == null)
            return;

        handCards.Remove(card);
        cardHand.RemoveCard(card);

        Destroy(card.gameObject);
    }
}
