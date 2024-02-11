using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SecurityManager : MonoBehaviour
{
    public static SecurityManager Instance;
    public float securityLevel;
    [SerializeField] private Image progressBarOfSecurityManager;

    [SerializeField] private TextMeshProUGUI amountText;
    public int sendAmount = 0;

    public Animator animationTab;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        securityLevel = 100;
        StartCoroutine(SecurityLevelAdjuster());
        UpdateUI();
    }

    private void Update()
    {
        progressBarOfSecurityManager.fillAmount = securityLevel / 100;
    }
    IEnumerator SecurityLevelAdjuster()
    {
        while (securityLevel > 1)
        {
            securityLevel--;
            yield return new WaitForSeconds(1f);
        }
        //DeadScene 
    }

    public void SendMilitary()
    {
        if (PlayerResource.Instance.militaryAmount > 0)
        {
            if (PlayerResource.Instance.militaryAmount >= sendAmount)
            {
                PlayerResource.Instance.militaryAmount -= sendAmount;
                securityLevel += sendAmount;
            }
            UpdateUI();
        }
        
    }

    public void AddAmount()
    {
        sendAmount++;
        UpdateUI();
    }
    public void DecreaseAmount()
    {
        if (sendAmount>0)
        {
            sendAmount--;
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        amountText.text = "Amount: " + sendAmount;
    }

    public void GoToSceneTab()
    {
        animationTab.SetTrigger("Go");
    }

    public void GoBack()
    {
        animationTab.SetTrigger("Back");
    }
}
