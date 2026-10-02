using UnityEngine;
using System;

public class CardPlayer : MonoBehaviour
{
    public static CardPlayer Instance { get; private set; }

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

    public void Play(CardData cardData, Cell origin)
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
