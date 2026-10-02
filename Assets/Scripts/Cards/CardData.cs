using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    [Header("Gameplay")]
    public int cost;
    public DamageData damageData;

    public CardTarget target;
    public CardArea area;
    public CardDamage damageType;

    public List<CardEffect> effects = new();

    [Header("Presentation")]
    public GameObject image;
    public GameObject vfx;
    public AudioClip sfx;

    public bool CanTarget(Cell cell)
    {
        return target != null && target.CanTarget(cell);
    }

    public void Play(Cell origin)
    {
        if (!CanTarget(origin))
            return;

        List<Cell> cells = area.GetCells(origin);

        if (damageType != null && damageData.maxDamage > 0)
            damageType.ApplyDamage(origin, cells, damageData);

        foreach (CardEffect effect in effects)
        {
            if (effect != null)
                effect.ApplyEffect(origin, cells, damageData, damageType, vfx);
        }
    }

    public void ApplyEffect(Cell origin, GameObject vfx)
    {
        if (!CanTarget(origin))
            return;

        List<Cell> cells = area.GetCells(origin);

        if (damageType != null && damageData.maxDamage > 0)
            damageType.ApplyDamage(
                origin,
                cells,
                damageData);

        foreach (CardEffect effect in effects)
        {
            if (effect != null)
                effect.ApplyEffect(
                    origin,
                    cells,
                    damageData,
                    damageType,
                    vfx);
        }
    }
}
