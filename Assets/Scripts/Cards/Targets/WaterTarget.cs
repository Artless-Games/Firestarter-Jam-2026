using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Targets/Water")]
public class WaterTarget : CardTarget
{
    public override bool CanTarget(Cell cell)
    {
        return cell != null && cell.IsWater;
    }
}
