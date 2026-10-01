using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Apply(Cell origin, List<Cell> cells);
}
