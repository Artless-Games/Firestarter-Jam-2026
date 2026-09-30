using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Targets/Empty")]
public class EmptyTarget : CardTarget
{
    public override bool CanTarget(Cell cell)
    {
        return cell != null && !cell.HasBuilding;
    }
}
