using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    [Header("Identity")]
    public string cardName;
    public Sprite image;
    public string description;

    [Header("Gameplay")]
    public int cost;
    public int damage;

    public CardTarget target;
    public CardArea area;

    public List<CardEffect> effects = new();

    public bool CanTarget(Cell cell)
    {
        return target != null && target.CanTarget(cell);
    }

    public void ApplyEffect(Cell cell)
    {
        if (!CanTarget(cell))
            return;

        //effect.Apply(cell);
    }
}
