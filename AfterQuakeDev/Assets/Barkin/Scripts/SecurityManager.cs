using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class SecurityManager : MonoBehaviour
{
    public static SecurityManager Instance;
    public float securityLevel;
    [SerializeField] private Image progressBarOfSecurityManager;

    public int sendAmount = 0;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        securityLevel = 100;
        StartCoroutine(SecurityLevelAdjuster());
    }

    private void Update()
    {
        progressBarOfSecurityManager.fillAmount = securityLevel / 100;
    }
    IEnumerator SecurityLevelAdjuster()
    {
        while (securityLevel > 0)
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
        }
        
    }

    public void AddAmount()
    {
        sendAmount++;
    }
    public void DecreaseAmount()
    {
        if (sendAmount>0)
        {
            sendAmount--;
        }
    }
}
