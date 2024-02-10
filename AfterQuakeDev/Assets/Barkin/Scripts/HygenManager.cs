using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HygenManager : MonoBehaviour
{
    private float hygienLevel;
    private float sumOfAllDeadPeopleInBuildings;

    [SerializeField] private Image progressBarForHygenManager;
    void Start()
    {
        StartCoroutine(HygienMeterTimer());
    }

    IEnumerator HygienMeterTimer()
    {
        while (hygienLevel < 60)
        {
            GameObject[] buildingObjects = GameObject.FindGameObjectsWithTag("Building");

            for (int i = 0; i < buildingObjects.Length; i++)
            {
                sumOfAllDeadPeopleInBuildings += buildingObjects[i].GetComponent<Building>().deadPeopleCount;
                yield return new WaitForSeconds(0.05f);
            }

            hygienLevel = sumOfAllDeadPeopleInBuildings / buildingObjects.Length;
            progressBarForHygenManager.fillAmount = hygienLevel / 60;
            Debug.Log(hygienLevel);
            yield return new WaitForSeconds(1f);
            sumOfAllDeadPeopleInBuildings = 0;
        }

        //DeadScene 
    }
    
    
}
