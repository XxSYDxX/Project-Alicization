using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameHandler : MonoBehaviour
{
    GameObject tmp, healthIndicator;
    GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectsWithTag("Manager")[0];

        tmp = transform.GetChild(2).gameObject;
        tmp.GetComponent<TextMeshPro>().text = this.gameObject.name;

        healthIndicator = transform.Find("HealthIndicator").gameObject;
    }

    void Update()
    {
        tmp.transform.rotation = gameManager.GetComponent<Commands>().activeCamera.transform.rotation;

        healthIndicator.transform.rotation = gameManager.GetComponent<Commands>().activeCamera.transform.rotation;

        if (this.gameObject.tag.Equals("Soul"))
        {
            tmp.SetActive(false);
            healthIndicator.SetActive(false);
        }
    }
}
