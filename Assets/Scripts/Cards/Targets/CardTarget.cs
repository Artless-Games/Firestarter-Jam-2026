using UnityEngine;

public abstract class CardTarget : ScriptableObject
{
    public abstract bool CanTarget(Cell cell);
}
