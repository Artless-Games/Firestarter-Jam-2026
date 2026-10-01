using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    [Header("Identity")]
    public string cardName;
    public Sprite image;

    [Header("Gameplay")]
    public int cost;
    public DamageData damageData;

    public CardTarget target;
    public CardArea area;
    public CardDamage damageType;

    public List<CardEffect> effects = new();

    public bool CanTarget(Cell cell)
    {
        return target != null && target.CanTarget(cell);
    }

    public void ApplyEffect(Cell origin)
    {
        if (!CanTarget(origin))
            return;

        List<Cell> cells = area.GetCells(origin);

        damageType.ApplyDamage(origin, cells, damageData);

        foreach (CardEffect effect in effects)
        {
            if (effect != null)
                effect.Apply(origin, cells);
        }
    }
}
