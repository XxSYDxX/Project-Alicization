using UnityEngine;

public class LootBoxHandler : MonoBehaviour
{
    public float buildingSkillRequired = 0f;
    public float buildingSkillGain = 0.5f;
    public float swordsmanshipGain = 0f;

    GameObject[] humans;

    void Start()
    {
        humans = GameObject.FindGameObjectsWithTag("Human");
    }

    void Update()
    {
        foreach (GameObject human in humans)
        {
            if (human.GetComponent<Stats>().buildingSkill >= buildingSkillRequired)
            {
                if (Vector3.Distance(human.transform.position, transform.position) < 3)
                {
                    human.GetComponent<Stats>().inventoryItems.Add(transform.name.Trim());

                    human.GetComponent<Stats>().buildingSkill += buildingSkillGain;
                    human.GetComponent<Stats>().swordsmanship += swordsmanshipGain;
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
