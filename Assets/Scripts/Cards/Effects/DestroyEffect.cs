using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Destroy")]
public class DestroyEffect : CardEffect
{
    public override void Apply(Cell cell)
    {
        if (cell == null || !cell.HasBuilding)
            return;

        cell.Building.DestroyBuilding();
    }
}
