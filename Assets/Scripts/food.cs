using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class food : MonoBehaviour
{
    public float health = 5f;
    public float despawnTimeLeft = 130f;

    Vector3 newDestination;
    NavMeshAgent agent;
    bool despawning = false;

    private float destructionCountDown = 10f;
    private string preSoulName;
    private float yRotation = 0;

    void Start()
    {
        preSoulName = this.gameObject.name;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, newDestination) <= 1)
            newDestination = DistantRandomLocation();

        else
            agent.SetDestination(newDestination);


        if (despawnTimeLeft > 0)
            despawnTimeLeft -= Time.deltaTime;

        else
        {
            despawning = true;
            health = 0f;
        }

        if (health <= 0f)
        {
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.tag = "Soul";

            if (this.gameObject.name == preSoulName)
                this.gameObject.name += " (Soul)";

            agent.speed = .5f;

            if (!despawning)
            {
                if (!transform.GetChild(1).gameObject.GetComponent<BioBoxHandler>().boxPickedUp)
                {
                    transform.GetChild(1).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.transform.rotation = Quaternion.Euler(0f,  yRotation, 0f);
                    yRotation -= Time.deltaTime * 50;
                }
                else
                {
                    transform.GetChild(1).gameObject.SetActive(false);
                }
            }

            if (destructionCountDown <= 0)
                Destroy(this.gameObject);

            destructionCountDown -= Time.deltaTime;
        }
    }

    Vector3 DistantRandomLocation()
    {
        Vector3 newPosition = new Vector3(UnityEngine.Random.Range(-290, 290), 0, UnityEngine.Random.Range(-290, 290));
        float distance = Vector3.Distance(newPosition, transform.position);

        while (distance < 5f || distance > 50f)
        {
            newPosition = new Vector3(UnityEngine.Random.Range(-290, 290), 0, UnityEngine.Random.Range(-290, 290));
            distance = Vector3.Distance(newPosition, transform.position);
        }
        return newPosition;
    }
}
