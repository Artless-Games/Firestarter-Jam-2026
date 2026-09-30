using System.Collections.Generic;
using UnityEngine;

public abstract class CardArea : ScriptableObject
{
    public abstract List<Cell> GetCells(Cell origin);
}
