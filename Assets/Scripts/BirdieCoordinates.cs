using UnityEngine;
using UnityEngine.UI;
using System;

public class BirdieCoordinates : MonoBehaviour
{
    public GameObject birdie;
    public Text coordinates;

    void Update()
    {
        string x = Convert.ToInt32(birdie.transform.position.x).ToString();
        string y = Convert.ToInt32(birdie.transform.position.y).ToString();
        string z = Convert.ToInt32(birdie.transform.position.z).ToString();

        string xyz = x + " " + y + " " + z;
        coordinates.text = xyz;
    }
}
