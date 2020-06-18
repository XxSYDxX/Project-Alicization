using System;
using System.Linq;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public float lookRadius = 15f;
    public float attackDamage = 3f;
    public float health = 7f;
    public float despawnTimeLeft = 150f;

    Transform target;
    NavMeshAgent agent;
    GameObject[] humans;
    Vector3 newDestination;

    private float destructionCountDown = 10f;
    private string preSoulName;
    private float yRotation;
    private bool despawning = false;

    void Start()
    {
        humans = GameObject.FindGameObjectsWithTag("Human");
        target = FindClosestTarget(humans);

        agent = GetComponent<NavMeshAgent>();

        newDestination = transform.position;
        preSoulName = this.gameObject.name;
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
            }

            if (distance <= 2.5)
            {
                if (target.GetComponent<Stats>().health > 0)
                {
                    if (!target.GetComponent<Stats>().underAttackOf.Contains(this.gameObject))
                        target.GetComponent<Stats>().underAttackOf.Add(this.gameObject);

                    target.GetComponent<Stats>().health -= attackDamage * Time.deltaTime;
                }

                else
                {
                    humans = GameObject.FindGameObjectsWithTag("Human");
                    target = FindClosestTarget(humans);
                }
            }
        }

        else if (distance > lookRadius)
        {
            if (target.GetComponent<Stats>().underAttackOf.Contains(this.gameObject))
                target.GetComponent<Stats>().underAttackOf.Remove(this.gameObject);

            humans = GameObject.FindGameObjectsWithTag("Human");
            target = FindClosestTarget(humans);

            if (Vector3.Distance(transform.position, newDestination) <= 1)
                newDestination = DistantRandomLocation(transform.position);
            else
                agent.SetDestination(newDestination);
        }


        if (despawnTimeLeft > 0)
            despawnTimeLeft -= Time.deltaTime;

        else
        {
            despawning = true;
            health = 0f;
        }


        if (health <= 0f)
        {
            if (target.GetComponent<Stats>().underAttackOf.Contains(this.gameObject))
                target.GetComponent<Stats>().underAttackOf.Remove(this.gameObject);

            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.tag = "Soul";

            if (this.gameObject.name == preSoulName)
                this.gameObject.name += " (Soul)";

            agent.speed = .5f;
            attackDamage = .25f;

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

    Transform FindClosestTarget(GameObject[] humans)
    {
        ArrayList distances = new ArrayList();
        foreach (GameObject human in humans)
        {
            float distance = Vector3.Distance(transform.position, human.gameObject.transform.position);
            distances.Add(distance);
        }

        float targetDistance = (float)distances.ToArray().Min();
        GameObject target = humans[distances.IndexOf(targetDistance)];

        return target.GetComponent<Transform>();
    }

    Vector3 DistantRandomLocation(Vector3 currentPosition)
    {
        Vector3 newPosition = new Vector3(UnityEngine.Random.Range(-290, 290), 0, UnityEngine.Random.Range(-290, 290));
        float distance = Vector3.Distance(newPosition, currentPosition);

        while (distance < 10f || distance > 50f)
        {
            newPosition = new Vector3(UnityEngine.Random.Range(-290, 290), 0, UnityEngine.Random.Range(-290, 290));
            distance = Vector3.Distance(newPosition, currentPosition);
        }
        return newPosition;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.localRotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
