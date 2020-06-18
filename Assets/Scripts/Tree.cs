using UnityEngine;

public class Tree : MonoBehaviour
{
    public float health = 3f;
    public float regenerationTime = 300f;
    public float regenerationCountDown;

    private float yRotation = 0;
    private float initialHealth;
    private bool isOnRegeneration = false;

    void Start()
    {
        initialHealth = health;
        regenerationCountDown = regenerationTime;
    }

    void Update()
    {
        if (!isOnRegeneration)
        {
            if (health <= 0)
            {
                isOnRegeneration = true;

                for (int i = 0; i < 13; i++)
                    transform.GetChild(i).gameObject.SetActive(false);

                transform.GetChild(13).gameObject.SetActive(true);
            }
        }

        else
        {
            transform.GetChild(13).gameObject.transform.rotation = Quaternion.Euler(0f,  yRotation, 0f);
            yRotation -= Time.deltaTime * 50;
        }


        if (regenerationCountDown <= regenerationTime && isOnRegeneration)
        {
            if (regenerationCountDown > 0)
                regenerationCountDown -= Time.deltaTime;

            else
            {
                regenerationCountDown = regenerationTime;
                health = initialHealth;
                isOnRegeneration = false;

                for (int i = 0; i < 13; i++)
                    transform.GetChild(i).gameObject.SetActive(true);

                transform.GetChild(13).gameObject.SetActive(false);
            }
        }
    }
}
