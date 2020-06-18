using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Stats : MonoBehaviour
{
    // current Stuffs
    public float health = 10f;
    public bool isOnAttack = false;
    public float despawnTimeLeft = 600f;
    public float happiness;
    public float horniness;
    public List<GameObject> underAttackOf;
    public float punchDamage = 0.1f;
    public float curiousityRate = 4f;
    public float hungerRate = 1f;

    // skills
    public float buildingSkill;
    public float swordsmanship;
    public float wisdom;
    public float sexAppeal;
    public float courage;
    public float sympathy;

    // Inventory
    public List<string> inventoryItems;

    // private stuffs
    private float destructionCountDown = 5f;
    private string preSoulName;
    private float previousSwordSkill = 0f;
    private bool despawning;
    private float yRotation = 0;
    private float previousHealth;
    float interest;

    void Update()
    {
        float[] interests = {buildingSkill, swordsmanship, wisdom};
        interest = interests.Max();

        if (underAttackOf.ToArray().Length > 0)
            isOnAttack = true;
        else
            isOnAttack = false;

        if (inventoryItems.ToArray().Length > 20)
            inventoryItems.RemoveAt(0);

        if (swordsmanship != previousSwordSkill)
        {
            punchDamage += (swordsmanship - previousSwordSkill) * 0.1f;

            previousSwordSkill = swordsmanship;
        }

        if (health < previousHealth)
        {
            hungerRate += (previousHealth - health);
            curiousityRate -= (previousHealth - health);

            previousHealth = health;
        }

        else if (health > previousHealth)
        {
            hungerRate -= (health - previousHealth) - courage * 0.005f;
            curiousityRate += (previousHealth - health) + interest * 0.005f;
            Debug.Log(interest);

            previousHealth = health;
        }

        if (despawnTimeLeft > 0)
            despawnTimeLeft -= Time.deltaTime;
        else
        {
            despawning = true;
            health = 0f;
        }

        if (health > 0)
            health -= Time.deltaTime / 60;

        if (health <= 0)
        {
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.tag = "Soul";

            if (this.gameObject.name == preSoulName)
                this.gameObject.name += " (Soul)";

            if (!despawning)
            {
                if (!transform.GetChild(1).gameObject.GetComponent<BioBoxHandler>().boxPickedUp)
                {
                    transform.GetChild(1).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.transform.rotation = Quaternion.Euler(0f,  yRotation, 0f);
                    yRotation -= Time.deltaTime * 50;
                }
                else
                    transform.GetChild(1).gameObject.SetActive(false);
            }

            if (destructionCountDown <= 0)
                Destroy(this.gameObject);

            destructionCountDown -= Time.deltaTime;
        }
    }

    void Start()
    {
        preSoulName = this.gameObject.name;
        previousHealth = health;

        underAttackOf = new List<GameObject>();
        inventoryItems = new List<string>();
    }
}
