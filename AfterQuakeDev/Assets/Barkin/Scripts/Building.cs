using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Building : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int howManyPeopleInThisBuilding;

    [SerializeField] private int deadPeopleCount;

    [SerializeField] private int injuredCount;
    private enum DamageType { NoDamage, Low, Medium, High }

    [SerializeField] private DamageType buildingDamageType;

    [SerializeField] private GameObject canvasOfBuilding;

    [SerializeField] private TextMeshProUGUI howManyPeopleInThisBuildingText;
    [SerializeField] private TextMeshProUGUI howManyDeadPeopleText;
    [SerializeField] private TextMeshProUGUI injuredPeopleText;
    [SerializeField] private TextMeshProUGUI buildingDamageTypeText;
    [SerializeField] private TextMeshProUGUI sendingAmountText;
    [SerializeField] private TextMeshProUGUI infoText;

    [SerializeField] private GameObject sendAfad;
    [SerializeField] private GameObject sendNgo;
    [SerializeField] private GameObject sendAmbulance;
    [SerializeField] private GameObject sendCrane;
    [SerializeField] private GameObject sendFuneralVehicle;
    [SerializeField] private GameObject sendFireFighter;

    private bool ngoRequired = true;

    private bool isFireActive = false;

    private bool isCraneNeeded = false;

    private bool isAmbulanceNeeded = false;

    private bool isFuneralVehicleNeeded = false;

    [SerializeField] private int sendAmount = 1;

    [SerializeField] private int waitingTimeForDeathIncrease;

    private int howManyAfadInThere;
    private int howManyNGOInThere;
    private int howManyAmbulanceInThere;
    private int howManyFuneralVehicleInThere;
    private int howManyCraneInThere;
    private int howManyFireFighterInThere;

    private bool clickedSendAfad = false;

    private int rescueTime=1;

    [SerializeField] private Image progressBar;

    [SerializeField] private int rescueTimeBasedOnDif;

    private bool progressBarStarted;
    void Start()
    {
        switch (buildingDamageType)
        {
            case DamageType.NoDamage:
                deadPeopleCount = 0;
                break;
            case DamageType.Low:
                deadPeopleCount = Random.Range(0, 10);
                rescueTimeBasedOnDif = Random.Range(30,50);
                break;
            case DamageType.Medium:
                deadPeopleCount = Random.Range(10, 30);
                rescueTimeBasedOnDif = Random.Range(50, 80);
                break;
            case DamageType.High:
                deadPeopleCount = Random.Range(30, 50);
                rescueTimeBasedOnDif = Random.Range(80, 120);
                break;
        }

        if (howManyPeopleInThisBuilding >= 80)
        {
            ngoRequired = false;
        }
        isFireActive = false;
        progressBarStarted = false;
        UpdateUI();
        StartCoroutine(GameplayLoop());

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        ClickedOnBuilding();
    }

    private void ClickedOnBuilding()
    {
        canvasOfBuilding.SetActive(true);
        UpdateUI();
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

    public void ClickedOnSendAfad()
    {
        if (PlayerResource.Instance.afadVolunteersAmount >= sendAmount && howManyPeopleInThisBuilding > 0)
        {
            howManyAfadInThere += sendAmount;
            clickedSendAfad = true;
            progressBarStarted = true;
            RescueSpeed(sendAmount);
            UpdateUI();
        }
        else
        {
            InfoTextTimer("You don't have enough AFAD volunteers!");
        }
    }
    public void ClickedOnSendNGO()
    {
        if (PlayerResource.Instance.ngoAmount >= sendAmount && howManyPeopleInThisBuilding > 0)
        {
            howManyNGOInThere += sendAmount;
            progressBarStarted = true;
            RescueSpeed(sendAmount);
            UpdateUI();
        }
        else
        {
            InfoTextTimer("You don't have enough amount of NGO people!");
        }
    }
    public void ClickedOnSendAmbulance()
    {
        if (PlayerResource.Instance.ambulanceAmount >= sendAmount)
        {
            howManyAmbulanceInThere += sendAmount;
            UpdateUI();
        }
        else
        {
            InfoTextTimer("You don't have enough amount of ambulance!");
        }
    }
    public void ClickedOnSendFuneralVehicle()
    {
        if (PlayerResource.Instance.funeralVehicleAmount >= sendAmount)
        {
            howManyFuneralVehicleInThere += sendAmount;
            UpdateUI();
        }
        else
        {
            InfoTextTimer("You don't have enough amount of funeral vehicle!");
        }
    }
    public void ClickedOnSendCrane()
    {
        if (PlayerResource.Instance.craneAmount >= sendAmount)
        {
            howManyCraneInThere += sendAmount;
            UpdateUI();
        }
        else
        {
            InfoTextTimer("You don't have enough amount of crane!");
        }
    }
    public void ClickedOnSendFireFighter()
    {
        if (PlayerResource.Instance.fireFighterAmount >= sendAmount)
        {
            howManyFireFighterInThere += sendAmount;
            UpdateUI();
        }
        else
        {
            InfoTextTimer("You don't have enough amount of fire fighter!");
        }
    }

    public void ClickedOnExit()
    {
        canvasOfBuilding.SetActive(false);
    }

    IEnumerator GameplayLoop()
    {
        while (buildingDamageType != DamageType.NoDamage)
        {
            if (howManyPeopleInThisBuilding > 0)
            {
                var timer = 0f;

                while (timer <= rescueTimeBasedOnDif / rescueTime)
                {
                    timer += Time.deltaTime;

                    progressBar.fillAmount = timer / (rescueTimeBasedOnDif / rescueTime);

                    yield return new WaitForEndOfFrame();
                }
                switch (buildingDamageType)
                {
                    case DamageType.Low:
                        int deadOrInjuryRandom = Random.Range(0, 18);
                        howManyPeopleInThisBuilding--;
                        if (deadOrInjuryRandom == 5)
                        {
                            deadPeopleCount++;
                        }
                        else
                        {
                            injuredCount++;
                        }
                        break;
                    case DamageType.Medium:
                        int deadOrInjuryRandomSecond = Random.Range(0, 8);
                        howManyPeopleInThisBuilding--;
                        if (deadOrInjuryRandomSecond == 5)
                        {
                            deadPeopleCount++;
                        }
                        else
                        {
                            injuredCount++;
                        }
                        break;
                    case DamageType.High:
                        int deadOrInjuryRandomThird = Random.Range(0, 3);
                        howManyPeopleInThisBuilding--;
                        if (deadOrInjuryRandomThird == 2)
                        {
                            deadPeopleCount++;
                        }
                        else
                        {
                            injuredCount++;
                        }
                        break;
                }
            }

            else
            {
                PlayerResource.Instance.afadVolunteersAmount += howManyAfadInThere;
                PlayerResource.Instance.ambulanceAmount += howManyAmbulanceInThere;
                PlayerResource.Instance.craneAmount += howManyCraneInThere;
                PlayerResource.Instance.fireFighterAmount += howManyFireFighterInThere;
                PlayerResource.Instance.ngoAmount += howManyNGOInThere;
                PlayerResource.Instance.funeralVehicleAmount += howManyFuneralVehicleInThere;
                break;
            }
            yield return new WaitForSeconds(1f);
            UpdateUI();
            progressBar.fillAmount = 0;
        }
    }

    void UpdateUI()
    {
        sendingAmountText.text = "Amount: " + sendAmount;
        howManyPeopleInThisBuildingText.text = "Number of Resident: " + howManyPeopleInThisBuilding.ToString();
        if (howManyAfadInThere>=1)
        {
            howManyDeadPeopleText.text = "Number of Dead: " + deadPeopleCount.ToString();
            injuredPeopleText.text = "Number of Injured: " + injuredCount.ToString();
        }
        else
        {
            howManyDeadPeopleText.text = "Number of Dead: ?";
            injuredPeopleText.text = "Number of Injured: ?";
        }
        buildingDamageTypeText.text = "Damage Type: " + buildingDamageType.ToString();


        if (howManyAfadInThere >= 1)
        {
            sendAmbulance.SetActive(true);
        }
        else
        {
            sendAmbulance.SetActive(false);
            sendFuneralVehicle.SetActive(false);
            sendCrane.SetActive(false);
            sendFireFighter.SetActive(false);
        }
        if (ngoRequired)
        {
            sendNgo.SetActive(true);
        }
        else
        { sendNgo.SetActive(false); }

        if (howManyAfadInThere >= 1 && buildingDamageType == DamageType.High)
        {
            sendCrane.SetActive(true);
        }
        else
        {
            sendCrane.SetActive(false);
        }

        if (deadPeopleCount > 0 && clickedSendAfad)
        {
            sendFuneralVehicle.SetActive(true);
        }
        else
        {
            sendFuneralVehicle.SetActive(false);
        }
    }

    IEnumerator InfoTextTimer(string textInfo)
    {
        infoText.text = textInfo;
        yield return new WaitForSeconds(3f);
        infoText.text = "";
    }
    
    private void RescueSpeed(int amount)
    {
        rescueTime += amount;
    }
}
