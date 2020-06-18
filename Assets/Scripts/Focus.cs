using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Focus : MonoBehaviour
{
    public float maxDistance = 7f;
    public GameObject hitObject;

    public GameObject vert;
    public GameObject diag;

    RaycastHit hit;
    GameObject manager;
    string lootBox;
    Material brick, iron, diamond;

    void Update()
    {
        Vector3 position = transform.GetChild(1).gameObject.transform.position;
        Vector3 direction = transform.GetChild(1).gameObject.transform.TransformDirection(Vector3.forward);
        float punchDamage = GetComponent<Stats>().punchDamage;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(position, direction, out hit, maxDistance))
            {
                hitObject = hit.collider.gameObject;

                if (hitObject.transform.parent != null)
                {
                    if (hitObject.transform.parent.tag.Equals("Tree"))
                        hitObject.transform.parent.gameObject.GetComponent<Tree>().health -= punchDamage;

                    if (hitObject.transform.parent.tag.Equals("Predator"))
                        hitObject.transform.parent.GetComponent<Zombie>().health -= punchDamage;

                    if (hitObject.transform.parent.tag.Equals("Food"))
                        hitObject.transform.parent.GetComponent<food>().health -= punchDamage;

                    if (hitObject.transform.parent.tag.Equals("Human"))
                        hitObject.transform.parent.GetComponent<Stats>().health -= punchDamage;
                }

                else
                {
                    if (hitObject.tag.Equals("BuildStuff"))
                        Destroy(hitObject);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(position, direction, out hit, maxDistance))
            {
                if (hit.transform.name.Equals("Ground"))
                {
                    Cursor.lockState = CursorLockMode.None;
                    transform.GetChild(1).GetComponent<MouseLook>().enabled = false;

                    manager.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                }

                if (hit.transform.tag.Equals("Human"))
                    manager.GetComponent<Commands>().ShowVesselStats(2, hit.collider.gameObject);
            }
        }
    }

    void OnVertClick()
    {
        GetComponent<Stats>().inventoryItems.Remove(lootBox);

        Vector3 position = new Vector3 (hit.point.x, 1.5f, hit.point.z);
        GameObject instant_vert = Instantiate(vert, position, transform.rotation);

        Renderer ren = instant_vert.GetComponent<Renderer>();

        switch (lootBox)
        {
            case "Brick":
                ren.material = brick;
                break;

            case "Iron":
                ren.material = iron;
                break;

            case "Diamond":
                ren.material = diamond;
                break;
        }

        OnEscClicked();
    }

    void OnDiagClick()
    {
        GetComponent<Stats>().inventoryItems.Remove(lootBox);

        Vector3 position = new Vector3 (hit.point.x, 1.33f, hit.point.z);

        Vector3 diagRotationVector = transform.rotation.eulerAngles;
        diagRotationVector.x = 45f;
        Quaternion rotation = Quaternion.Euler(diagRotationVector + transform.TransformDirection(Vector3.up));

        GameObject instant_diag = Instantiate(diag, position, rotation);

        Renderer ren = instant_diag.GetComponent<Renderer>();

        switch (lootBox)
        {
            case "Brick":
                ren.material = brick;
                break;

            case "Iron":
                ren.material = iron;
                break;

            case "Diamond":
                ren.material = diamond;
                break;
        }

        OnEscClicked();
    }

    void BuildSelector(string lootBoxName)
    {
        lootBox = lootBoxName;

        if (GetComponent<Stats>().inventoryItems.Contains(lootBoxName))
        {
            manager.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            manager.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            manager.GetComponent<Commands>().Display("You don't have any " + lootBoxName);
            OnEscClicked();
        }
    }

    void Start()
    {
        manager = GameObject.FindGameObjectsWithTag("Manager")[0].gameObject;

        Transform MaterialPanel = manager.transform.GetChild(1).GetChild(0);
        Transform WallPanel = manager.transform.GetChild(1).GetChild(1);

        MaterialPanel.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(OnBrickClick);
        MaterialPanel.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(OnIronClick);
        MaterialPanel.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(OnDiamondClick);

        WallPanel.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(OnVertClick);
        WallPanel.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(OnDiagClick);

        MaterialPanel.GetChild(4).gameObject.GetComponent<Button>().onClick.AddListener(OnEscClicked);
        WallPanel.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(OnEscClicked);

        brick = Resources.Load<Material>("Materials/Bricks");
        iron = Resources.Load<Material>("Materials/Iron");
        diamond = Resources.Load<Material>("Materials/Diamond");
    }

    void OnEscClicked()
    {
        transform.GetChild(1).GetComponent<MouseLook>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        manager.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        manager.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
    }

    void OnBrickClick()
    {
        BuildSelector("Brick");
    }

    void OnDiamondClick()
    {
        BuildSelector("Diamond");
    }

    void OnIronClick()
    {
        BuildSelector("Iron");
    }
}
