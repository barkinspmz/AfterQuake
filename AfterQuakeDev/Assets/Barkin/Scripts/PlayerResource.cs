using TMPro;
using UnityEngine;

public class PlayerResource : MonoBehaviour
{
    public static PlayerResource Instance { get; private set; }

    public int ambulanceAmount;
    public int afadVolunteersAmount;
    public int militaryAmount;
    public int craneAmount;
    public int ngoAmount; 
    public int funeralVehicleAmount;
    public int fireFighterAmount;

    public TextMeshProUGUI AFADText;
    public TextMeshProUGUI NGOText;
    public TextMeshProUGUI ambulanceText;
    public TextMeshProUGUI militaryText;
    public TextMeshProUGUI craneText;
    public TextMeshProUGUI funeralVehicleText;
    public TextMeshProUGUI fireFighterText;


    private void Awake()
    {
        Instance = this;
    }

    public void UpdateUI()
    {
        AFADText.text = afadVolunteersAmount.ToString();
        NGOText.text = ngoAmount.ToString();
        ambulanceText.text = ambulanceAmount.ToString();
        militaryText.text = militaryAmount.ToString();
        craneText.text = craneAmount.ToString();
        funeralVehicleText.text = funeralVehicleAmount.ToString();
        fireFighterText.text = fireFighterAmount.ToString();
    }
}
