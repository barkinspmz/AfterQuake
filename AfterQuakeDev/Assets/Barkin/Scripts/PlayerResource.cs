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
    
    private void Awake()
    {
        Instance = this;
    }
}
