using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioBoxHandler : MonoBehaviour
{
    public float healthGain = 1f;
    public float courageGain = .25f;
    public float swordsmanshipGain = .1f;

    public bool boxPickedUp = false;

    GameObject[] humans;

    void Start()
    {
        humans = GameObject.FindGameObjectsWithTag("Human");
    }

    void Update()
    {
        foreach (GameObject human in humans)
        {
            if (Vector3.Distance(human.transform.position, transform.position) < 3)
            {
                human.GetComponent<Stats>().health += healthGain;
                human.GetComponent<Stats>().courage += courageGain;
                human.GetComponent<Stats>().swordsmanship += swordsmanshipGain;
                boxPickedUp = true;
            }
        }
    }
}
