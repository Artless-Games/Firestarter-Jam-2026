using UnityEngine;
using System;
using System.Collections;

public class CardPlayer : MonoBehaviour
{
    // Singleton
    public static CardPlayer Instance { get; private set; }

    public float cardShowDuration = 0.5f;
    public float cardDisplayTime = 0.5f;

    public event Action Finished;
    private Cell origin;
    private CardData currentCard;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Play(CardData cardData, Cell origin, Card card)
    {
        if (cardData == null || origin == null || card == null)
            return;

        StartCoroutine(PlayRoutine(cardData, origin, card));
    }

    private IEnumerator PlayRoutine(
        CardData cardData,
        Cell origin,
        Card card)
    {
        currentCard = cardData;
        this.origin = origin;

        yield return card.ShowFullscreen(cardShowDuration);

        yield return new WaitForSeconds(cardDisplayTime);

        yield return card.HideFullscreen(cardShowDuration);

        PlayEffect(cardData, origin);
    }

    public void PlayEffect(CardData cardData, Cell origin)
    {
        if (cardData == null || origin == null)
            return;

        currentCard = cardData;
        this.origin = origin;

        GameObject vfx = null;

        if (cardData.vfx != null)
        {
            vfx = Instantiate(
                cardData.vfx,
                origin.transform.position,
                Quaternion.identity);

            
            if (!vfx.TryGetComponent<CardVFX>(out var cardVFX))
                cardVFX = vfx.AddComponent<CardVFX>();

            cardVFX.Finished += OnVFXFinished;
        }

        cardData.ApplyEffect(origin, vfx);

        if (cardData.sfx != null)
        {
            AudioSource.PlayClipAtPoint(
                cardData.sfx,
                origin.transform.position);
        }
    }

    private void OnVFXFinished()
    {
        if (origin != null &&
            origin.HasBuilding &&
            origin.Building.IsOnFire)
        {
            origin.Building.Extinguish();
        }

        origin = null;
        currentCard = null;

        Finished?.Invoke();
    }
}
