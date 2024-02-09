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

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        ambulanceAmount = 100;
        afadVolunteersAmount = 100;
        militaryAmount = 100;
        craneAmount = 100;
        ngoAmount = 100;
        funeralVehicleAmount = 100;
    }
}
