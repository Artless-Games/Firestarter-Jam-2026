using TMPro;
using UnityEngine;

public class TurnUI : MonoBehaviour
{
    [SerializeField] private TMP_Text turnText;

    private void Update()
    {
        if (TurnManager.Instance == null)
            return;

        turnText.text =
            $"{TurnManager.Instance.Turn} / {TurnManager.Instance.maxTurns}";
    }
}
