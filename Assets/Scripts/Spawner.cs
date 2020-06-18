using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnInterval = 120f;
    public float foodFrequency = 10f;
    public float predatorFrequency = 5f;

    public GameObject pig;
    public GameObject cow;
    public GameObject zombie;

    private float nextSpawningTimeLeft;
    private Vector3 lastRandomLocation;
    private int cowID = 1;
    private int pigID = 1;
    private int zombieID = 1;

    void Start()
    {
        nextSpawningTimeLeft = spawnInterval;
        lastRandomLocation = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (nextSpawningTimeLeft == spawnInterval)
        {
            for (int i = 0; i < foodFrequency/2; i++)
            {
                GameObject cowInstance = Instantiate(cow, RandomLocation(), transform.rotation);
                cowInstance.name = "c" + cowID;
                cowID++;

                GameObject pigInstance = Instantiate(pig, RandomLocation(), transform.rotation);
                pigInstance.name = "p" + pigID;
                pigID++;
            }

            for (int i = 0; i < predatorFrequency; i++)
            {
                GameObject zombieInstance = Instantiate(zombie, RandomLocation(), transform.rotation);
                zombieInstance.name = "z" + zombieID;
                zombieID++;
            }
        }

        if (nextSpawningTimeLeft > 0)
            nextSpawningTimeLeft -= Time.deltaTime;
        else
            nextSpawningTimeLeft = spawnInterval;
    }

    Vector3 RandomLocation()
    {
        Vector3 randomLocation = new Vector3(UnityEngine.Random.Range(-270, 270), 0, UnityEngine.Random.Range(-270, 270));

        while (Vector3.Distance(randomLocation, lastRandomLocation) < 100f)
            randomLocation = new Vector3(UnityEngine.Random.Range(-270, 270), 0, UnityEngine.Random.Range(-270, 270));

        return randomLocation;
    }
}
