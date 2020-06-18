using System;
using UnityEngine;
using TMPro;

public class HealthIndicator : MonoBehaviour
{
    void Update()
    {
        TextMeshPro healthIndicatorTMP = transform.Find("HealthIndicator").gameObject.GetComponent<TextMeshPro>();

        switch(transform.tag)
        {
            case "Human":
                healthIndicatorTMP.text = Math.Round(GetComponent<Stats>().health, 3).ToString();
                break;

            case "Predator":
                healthIndicatorTMP.text = Math.Round(GetComponent<Zombie>().health, 3).ToString();
                break;

            case "Food":
                healthIndicatorTMP.text = Math.Round(GetComponent<food>().health, 3).ToString();
                break;
        }
    }
}
