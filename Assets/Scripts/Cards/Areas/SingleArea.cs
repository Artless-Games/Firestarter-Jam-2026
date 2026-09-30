using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Areas/Single")]
public class SingleArea : CardArea
{
    public override List<Cell> GetCells(Cell origin)
    {
        return new List<Cell> { origin };
    }
}
