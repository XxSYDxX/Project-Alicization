using UnityEngine;
using System.Linq;

public class HumanBrain : MonoBehaviour
{
    List<GameObject> food;

    void Update()
    {
        Stats stats = transform.GetComponent<Stats>();

        if (stats.health < 7)
        {
            if (stats.hungerRate > stats.curiousityRate)
            {

            }
        }
    }

    void Start()
    {
        food = 
    }
}
