using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Building : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int howManyPeopleInThisBuilding;

    [SerializeField] private int deadPeopleCount;
    private enum DamageType { NoDamage, Low, Medium, High }

    [SerializeField] private DamageType buildingDamageType;

    [SerializeField] private GameObject canvasOfBuilding;

    [SerializeField] private TextMeshProUGUI howManyPeopleInThisBuildingText;
    [SerializeField] private TextMeshProUGUI howManyDeadPeopleText;
    [SerializeField] private TextMeshProUGUI buildingDamageTypeText;
    [SerializeField] private TextMeshProUGUI sendingAmountText;

    private bool afadRequired = false;

    private bool isFireActive = false;

    [SerializeField] private int sendAmount = 1;

    [SerializeField] private int waitingTimeForDeathIncrease;
    void Start()
    {
        switch (buildingDamageType)
        {
            case DamageType.NoDamage:
                deadPeopleCount = 0;
                break;
            case DamageType.Low:
                deadPeopleCount = Random.Range(0, 10);
                break;
            case DamageType.Medium:
                deadPeopleCount = Random.Range(10, 30);
                break;
            case DamageType.High:
                deadPeopleCount = Random.Range(20, 50);
                break;
        }

        if (howManyPeopleInThisBuilding >= 80)
        {
            afadRequired = true;
        }
        isFireActive = false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        ClickedOnBuilding();
    }

    private void ClickedOnBuilding()
    {
        canvasOfBuilding.SetActive(true);
        howManyPeopleInThisBuildingText.text = "Number of Resident: " + howManyPeopleInThisBuilding.ToString();
        howManyDeadPeopleText.text = "Number of Dead: " + deadPeopleCount.ToString();
        buildingDamageTypeText.text = "Damage Type: " + buildingDamageType.ToString();
    }

    public void ClickedOnIncreaseButton()
    {
        sendAmount++;
        sendingAmountText.text = "Amount: " + sendAmount.ToString();
    }

    public void ClickedOnDecreaseButton()
    {
        if (sendAmount>1)
        {
            sendAmount--;
            sendingAmountText.text = "Amount: " + sendAmount.ToString();
        }
    }

    public void ClickedOnExit()
    {
        canvasOfBuilding.SetActive(false);
    }

    IEnumerator GameplayLoop()
    {
        while (true)
        {

        }
    }
}
