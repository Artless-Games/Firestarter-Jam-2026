using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Targets/Border")]
public class BorderTarget : CardTarget
{
    public override bool CanTarget(Cell cell)
    {
        if (cell == null)
            return false;

        Vector2Int coordinates = cell.Coordinates;

        return coordinates.x == 0 ||
               coordinates.x == GridManager.Instance.width - 1 ||
               coordinates.y == 0 ||
               coordinates.y == GridManager.Instance.height - 1;
    }
}
