using TMPro;
using UnityEngine;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text energyText;

    private void Update()
    {
        energyText.text =
            TurnManager.Instance.Energy.ToString();
    }
}
