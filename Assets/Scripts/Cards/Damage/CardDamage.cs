using System.Collections.Generic;
using UnityEngine;

public abstract class CardDamage : ScriptableObject
{
    public abstract void ApplyDamage(Cell origin, List<Cell> cells, DamageData damage);
}
