using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Targets/Building")]
public class BuildingTarget : CardTarget
{
    public override bool CanTarget(Cell cell)
    {
        return cell != null && cell.HasBuilding;
    }
}
