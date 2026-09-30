using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Targets/Any")]
public class AnyTarget : CardTarget
{
    public override bool CanTarget(Cell cell)
    {
        return cell != null;
    }
}
