using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite artwork;

    public CardTarget target;
    public CardEffect effect;

    public bool CanTarget(Cell cell)
    {
        return target != null && target.CanTarget(cell);
    }

    public void ApplyEffect(Cell cell)
    {
        if (!CanTarget(cell))
            return;

        effect.Apply(cell);
    }
}
