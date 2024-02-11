using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Building : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int howManyPeopleInThisBuilding;

    public int deadPeopleCount;

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
    [SerializeField] private Image progressBarForAmbulance;

    [SerializeField] private int rescueTimeBasedOnDif;

    [SerializeField] private GameObject textAmbulance;
    [SerializeField] private GameObject ambulanceBGProgress;

    [SerializeField] private Image[] icons = new Image[6];
    //0 AFAD
    //1 NGO
    //2 Ambulance
    //3 Funeral
    //4 Crane
    //5 Fire Fighter

    [SerializeField] Sprite[] spritesForIcons = new Sprite[6];

    private int lockForFuneralSystem;

    private int lockForAmbulanceProgress;
    private bool progressBarStarted;

    [SerializeField] private Image progressBarForFuneralVehicle;

    [SerializeField] private GameObject backgroundFuneralVehicleBar;
    [SerializeField] private GameObject backgroundFuneralText;

    [SerializeField] private Image progressBarForCrane;

    [SerializeField] private GameObject backgroundForCrane;
    [SerializeField] private GameObject backgroundForCraneText;

    private int lockForCraneGameplay = 0;

    [SerializeField] private GameObject fireParticle;

    private int lockForFireFighterGameplay = 0;

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

                var isThereFireRandomNum = Random.Range(0, 20);
                if (isThereFireRandomNum == 5)
                {
                    isFireActive = true;
                    StartCoroutine(ThereIsFireTimer());
                }
                break;
            case DamageType.Medium:
                deadPeopleCount = Random.Range(10, 30);
                rescueTimeBasedOnDif = Random.Range(50, 80);
                var isThereFireRandomNumSec = Random.Range(0, 12);
                if (isThereFireRandomNumSec == 5)
                {
                    isFireActive = true;
                    StartCoroutine(ThereIsFireTimer());
                }
                break;
            case DamageType.High:
                deadPeopleCount = Random.Range(30, 50);
                rescueTimeBasedOnDif = Random.Range(80, 120);
                var isThereFireRandomNumThird = Random.Range(0, 8);
                if (isThereFireRandomNumThird == 5)
                {
                    isFireActive = true;
                    StartCoroutine(ThereIsFireTimer());
                }
                break;
        }

        if (howManyPeopleInThisBuilding >= 80)
        {
            ngoRequired = false;
        }
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
            PlayerResource.Instance.UpdateUI();
            PlayerResource.Instance.afadVolunteersAmount -= sendAmount;
            clickedSendAfad = true;
            progressBarStarted = true;
            RescueSpeed(sendAmount);
            UpdateUI();
            icons[0].enabled = true;
            if (icons[0].sprite == null)
            {
                icons[0].sprite = spritesForIcons[0];
            }
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
            PlayerResource.Instance.ngoAmount -= sendAmount;
            progressBarStarted = true;
            RescueSpeed(sendAmount);
            UpdateUI();
            PlayerResource.Instance.UpdateUI();
            icons[1].enabled = true;
            if (icons[1].sprite == null)
            {
                icons[1].sprite = spritesForIcons[1];
            }
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
            PlayerResource.Instance.ambulanceAmount -= sendAmount;
            UpdateUI();
            PlayerResource.Instance.UpdateUI();
            icons[2].enabled = true;
            if (icons[2].sprite == null)
            {
                icons[2].sprite = spritesForIcons[2];
            }

            lockForAmbulanceProgress++;
            if (lockForAmbulanceProgress == 1)
            {
                StartCoroutine(AmbulanceGameplayTimer());
                ambulanceBGProgress.SetActive(true);
                textAmbulance.SetActive(true);
            }
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
            PlayerResource.Instance.funeralVehicleAmount -= sendAmount;
            UpdateUI();
            PlayerResource.Instance.UpdateUI();
            icons[3].enabled = true;
            if (icons[3].sprite == null)
            {
                icons[3].sprite = spritesForIcons[3];
            }
            lockForFuneralSystem++;

            if (lockForFuneralSystem == 1)
            {
                StartCoroutine(FuneralVehicleGameplay());
                backgroundFuneralVehicleBar.SetActive(true);
                backgroundFuneralText.SetActive(true);
            }
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
            PlayerResource.Instance.craneAmount -= sendAmount;
            UpdateUI();
            PlayerResource.Instance.UpdateUI();
            icons[4].enabled = true;
            lockForCraneGameplay++;
            if (lockForCraneGameplay == 1)
            {
                StartCoroutine(CraneGameplay());
                backgroundForCrane.SetActive(true);
                backgroundForCraneText.SetActive(true);
            }
            if (icons[4].sprite == null)
            {
                icons[4].sprite = spritesForIcons[4];
            }
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
            PlayerResource.Instance.fireFighterAmount -= sendAmount;
            UpdateUI();
            PlayerResource.Instance.UpdateUI();
            icons[5].enabled = true;
            if (icons[5].sprite == null)
            {
                icons[5].sprite = spritesForIcons[5];
            }

            lockForFireFighterGameplay++;
            if (lockForFireFighterGameplay == 1)
            {
                StartCoroutine (FireFighterGameplay());
            }
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

                        if (injuredCount>0)
                        {
                            int injuredDeadOrNot = Random.Range(0, 10);
                            if (injuredDeadOrNot == 5)
                            {
                                injuredCount--;
                                deadPeopleCount++;
                            }
                            else
                            {
                                injuredCount++;
                            }
                        }
                        break;
                    case DamageType.Medium:
                        int deadOrInjuryRandomSecond = Random.Range(0, 10);
                        howManyPeopleInThisBuilding--;
                        if (deadOrInjuryRandomSecond == 5)
                        {
                            deadPeopleCount++;
                        }
                        else
                        {
                            injuredCount++;
                        }


                        if (injuredCount > 0)
                        {
                            int injuredDeadOrNotSecond = Random.Range(0, 7);
                            if (injuredDeadOrNotSecond == 3)
                            {
                                injuredCount--;
                                deadPeopleCount++;
                            }
                            else
                            {
                                injuredCount++;
                            }
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

                        if (injuredCount > 0)
                        {
                            int injuredDeadOrNotThird = Random.Range(0, 4);
                            if (injuredDeadOrNotThird == 2)
                            {
                                injuredCount--;
                                deadPeopleCount++;
                            }
                            else
                            {
                                injuredCount++;
                            }
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

        if (isFireActive)
        {
            sendFireFighter.SetActive(true);
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

    IEnumerator AmbulanceGameplayTimer()
    {
        while (true)
        {
            var timer = 0f;

            while (timer <= rescueTimeBasedOnDif / howManyAmbulanceInThere)
            {
                timer += Time.deltaTime;

                progressBarForAmbulance.fillAmount = timer / (rescueTimeBasedOnDif / howManyAmbulanceInThere);

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(1f);
            if (howManyAmbulanceInThere > 0)
            {
                if (injuredCount >= howManyAmbulanceInThere)
                {
                    injuredCount -= howManyAmbulanceInThere;
                }
                
                if (injuredCount < howManyAmbulanceInThere)
                {
                    injuredCount = 0;
                }
               
            }
            progressBarForAmbulance.fillAmount = 0;
            UpdateUI();
        }
    }

    IEnumerator FuneralVehicleGameplay()
    {

        while (true)
        {
            var timer = 0f;

            while (timer <= rescueTimeBasedOnDif / howManyFuneralVehicleInThere)
            {
                timer += Time.deltaTime;

                progressBarForFuneralVehicle.fillAmount = timer / (rescueTimeBasedOnDif / howManyFuneralVehicleInThere);

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(1f);

            if (howManyFuneralVehicleInThere > 0)
            {
                if (deadPeopleCount >= howManyFuneralVehicleInThere)
                {
                    deadPeopleCount -= howManyFuneralVehicleInThere;
                }

                if (deadPeopleCount < howManyFuneralVehicleInThere)
                {
                    deadPeopleCount = 0;
                }
            }
            progressBarForFuneralVehicle.fillAmount = 0;
            UpdateUI();
        }
    }

    IEnumerator CraneGameplay()
    {
        while (true)
        {
            var timer = 0f;

            while (timer <= rescueTimeBasedOnDif / howManyCraneInThere)
            {
                timer += Time.deltaTime;

                progressBarForCrane.fillAmount = timer / (rescueTimeBasedOnDif / howManyCraneInThere);

                yield return new WaitForEndOfFrame();
            }

            progressBarForCrane.fillAmount = 0;
            UpdateUI();
        }
    }

    IEnumerator ThereIsFireTimer()
    {
        fireParticle.SetActive(true);
        while (isFireActive)
        {
            yield return new WaitForSeconds(10f);
            if (howManyPeopleInThisBuilding > 0)
            {
                howManyPeopleInThisBuilding--;
                deadPeopleCount++;
            }
        }
    }
    
    IEnumerator FireFighterGameplay()
    {
        var timer = 0f;

        while (timer <= 120 / howManyFireFighterInThere)
        {
            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        isFireActive = false;
        fireParticle.SetActive(false);
    }
}
