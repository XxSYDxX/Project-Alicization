using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Commands : MonoBehaviour
{
    public string commandlist = "Commands:\n/stats (or s); /generate <object> <X,Y,Z>; /destroy <object> <number>; /c (or switchcam); /kill <mobName>";


    // #######################################################################################################
    // Supreme Access
    public string Generate(String objectName, String position)
    {
        String[] sptr = {","};
        String[] XYZ = position.Split(sptr, StringSplitOptions.RemoveEmptyEntries);
        float x = float.Parse(XYZ[0]);
        float y = float.Parse(XYZ[1]);
        float z = float.Parse(XYZ[2]);

        switch (objectName)
        {
            case "zombie":
                Instantiate(zombie, new Vector3(x, y, z), transform.rotation);
                return "zombie genned";

            case "pig":
                Instantiate(pig, new Vector3(x, y, z), transform.rotation);
                return "zombie genned";

            case "cow":
                Instantiate(cow, new Vector3(x, y, z), transform.rotation);
                return "zombie genned";

            default:
                return objectName + " not found. Try zombie, pig or cow.";
        }
    }

    public string Destroy()
    {
        return "Destruction 100";
    }

    public string Kill(string mobName)
    {
        GameObject mob = GameObject.Find(mobName);

        if (humans.Contains(mob))
        {
            mob.GetComponent<Stats>().health = 0;
            return mobName + " killed";
        }

        else if (zombies.Contains(mob))
        {
            mob.GetComponent<Zombie>().health = 0;
            return mobName + " killed";
        }

        else if (foods.Contains(mob))
        {
            mob.GetComponent<food>().health = 0;
            return mobName + " killed";
        }

        else
            return "None named " + mobName + " was found alive";
    }


    // ###########################################################################################################
    // Command Navigation
    void HandleCommands()
    {
        foreach (GameObject human in GameObject.FindGameObjectsWithTag("Human"))
            humans.Add(human);

        foreach (GameObject zombie in GameObject.FindGameObjectsWithTag("Predator"))
            zombies.Add(zombie);

        foreach (GameObject food in GameObject.FindGameObjectsWithTag("Food"))
            foods.Add(food);

        String[] sptr = {" "};
        String[] args = putInCommand.Split(sptr, StringSplitOptions.RemoveEmptyEntries);

        switch(args[0].Trim())
        {
            case "stats":
            case "s":
                ShowVesselStats(1, instantiatedVessel);
                break;


            case "generate":
                if (args.Length >= 3)
                {
                    if (args[1].Trim().Equals("vessel"))
                        Display(GenerateVessel(args[2]));
                    else
                        Display(Generate(args[1].Trim(), args[2].Trim()));
                }
                else
                    Display("Not Enough arguments for generation");
                    break;


            case "destroy":
                if (args.Length >= 3)
                {
                    if (args[1].Trim().Equals("vessel"))
                        Display(DestroyVessel());
                    else
                        Display(Destroy());
                }
                else
                    Display("Not enough arguments for generation");
                break;


            case "c":
            case "switchcam":
                Display(SwitchCamera());
                break;


            case "kill":
                if (args.Length >= 2)
                    Display( Kill(args[1].Trim()) );
                else
                    Display("Not enough arguments");
                break;


            case "help":
                Cursor.lockState = CursorLockMode.None;
                helpPanel.SetActive(true);
                helpPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = commandlist;
                helpPanelButton.GetComponent<Button>().onClick.AddListener(OnHelpButtonClicked);
                break;


            default:
                Display("Command Unrecognized\n Check out for spelling or unnecessary spaces.");
                break;
        }
    }

    // ###########################################################################################################
    // this class Declarations
    public GameObject vessel;
    public GameObject inputFieldObject;
    public GameObject displayText;
    public GameObject helpPanel;
    public GameObject BirdCam;
    public GameObject BirdXYZ;
    public Button helpPanelButton;
    public GameObject activeCamera;
    public GameObject zombie, pig, cow;

    private bool commandsOn = false;
    private string putInCommand;
    private InputField inputField;
    private float displayTimeOut = 3f;
    private bool timeDisplayOut = false;
    private bool vesselExists = false;
    private bool birdCamOn = false;
    private GameObject instantiatedVessel;
    private GameObject humanForStats;

    List<GameObject> humans = new List<GameObject>();
    List<GameObject> zombies = new List<GameObject>();
    List<GameObject> foods = new List<GameObject>();


    // ########################################################################################################
    // Classy Stuffs:
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            commandsOn = true;
            Cursor.lockState = CursorLockMode.None;
            inputFieldObject.SetActive(true);
            inputField.Select();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && commandsOn)
        {
            commandsOn = false;
            inputFieldObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.Return) && commandsOn)
        {
            commandsOn = false;
            inputField.text = "";
            inputFieldObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            if (putInCommand.Length != 0)
                HandleCommands();
        }

        if (displayTimeOut > 0 && timeDisplayOut)
            displayTimeOut -= Time.deltaTime;

        else if (displayTimeOut < 0)
        {
            timeDisplayOut = false;
            displayText.SetActive(false);
            displayTimeOut = 3f;
        }

        if (vesselExists)
        {
            if (instantiatedVessel.GetComponent<Stats>().health <= 0)
            {
                string killers = "";
                foreach(GameObject killer in instantiatedVessel.GetComponent<Stats>().underAttackOf.ToArray())
                    killers += killer.name + " ";

                Display("Vessel was killed by " + killers);
                instantiatedVessel.transform.GetChild(1).gameObject.SetActive(false);
                BirdCam.SetActive(true);
                BirdXYZ.SetActive(true);
                activeCamera = BirdCam.transform.GetChild(0).gameObject;
                vesselExists = false;
            }
        }
    }

    void Start()
    {
        inputField = inputFieldObject.GetComponent<InputField>();
        inputField.onEndEdit.AddListener(GetTypedCommand);
        activeCamera = BirdCam.transform.GetChild(0).gameObject;

        Transform statsPanel = transform.GetChild(2).GetChild(0);
        statsPanel.GetChild(2).GetComponent<Button>().onClick.AddListener(OnEscClicked);
        statsPanel.GetChild(3).GetComponent<Button>().onClick.AddListener(currentVesselStats);
        statsPanel.GetChild(4).GetComponent<Button>().onClick.AddListener(vesselSkillStats);
        statsPanel.GetChild(5).GetComponent<Button>().onClick.AddListener(vesselInventoryStats);
    }

    void currentVesselStats()
    {
        Transform statsPanel = transform.GetChild(2).GetChild(0);
        statsPanel.GetChild(0).GetComponent<Text>().text = "Current Stats";

        Stats stats = humanForStats.GetComponent<Stats>();

        Vector3 position = humanForStats.transform.position;
        string x = Convert.ToInt32(position.x).ToString();
        string y = Convert.ToInt32(position.y).ToString();
        string z = Convert.ToInt32(position.z).ToString();
        string coords = x + " " + y + " " + z;

        string desc = string.Format(
            "Coords: {2}; Punch Damage: {0}; Time Left to Live: T-{1}s;\n\nHappiness: {3}; Horniness {4}; Hunger: {5}; Curiousity: {6}; ",
            Math.Round(stats.punchDamage, 3),
            Convert.ToInt32(stats.despawnTimeLeft),
            coords,
            Math.Round(stats.happiness, 3),
            Math.Round(stats.horniness, 3),
            Math.Round(stats.hungerRate, 3),
            Math.Round(stats.curiousityRate, 3)
        );

        statsPanel.GetChild(1).GetComponent<Text>().text = desc;
    }

    void vesselInventoryStats()
    {
        Transform statsPanel = transform.GetChild(2).GetChild(0);
        statsPanel.GetChild(0).GetComponent<Text>().text = "Inventory Items";

        Stats stats = humanForStats.GetComponent<Stats>();
        string[] items = stats.inventoryItems.ToArray();

        string desc = "";
        foreach (string item in items)
            desc += item + "; ";

        statsPanel.GetChild(1).GetComponent<Text>().text = desc;
    }

    void vesselSkillStats()
    {
        Transform statsPanel = transform.GetChild(2).GetChild(0);
        statsPanel.GetChild(0).GetComponent<Text>().text = "Skills";

        Stats stats = humanForStats.GetComponent<Stats>();

        string desc = string.Format(
            "Swordsmanship level: {1}; Wisdom Gradient: {2}; Building Skill Score: {3};\n\nCourage: {0}; Sympathy: {4}, Sex Appeal: {5}",
            stats.courage,
            stats.swordsmanship,
            stats.wisdom,
            stats.buildingSkill,
            stats.sympathy,
            stats.sexAppeal
        );

        statsPanel.GetChild(1).GetComponent<Text>().text = desc;
    }

    public void ShowVesselStats(int type, GameObject humanSelectedForStats)
    {
        if (type == 1)
        {
            if (vesselExists)
            {
                humanForStats = instantiatedVessel;

                Cursor.lockState = CursorLockMode.None;
                instantiatedVessel.transform.GetChild(1).GetComponent<MouseLook>().enabled = false;

                transform.GetChild(2).GetChild(0).gameObject.SetActive(true);

                currentVesselStats();
            }
            else
                Display("No active vessel to show the stats of. Generate a vessel first. To check the stats of the humans within a distance of 7 units, right cick on them while in a vessel.");
        }

        else if (type == 2)
        {
            humanForStats = humanSelectedForStats;
            Cursor.lockState = CursorLockMode.None;
            instantiatedVessel.transform.GetChild(1).GetComponent<MouseLook>().enabled = false;

            transform.GetChild(2).GetChild(0).gameObject.SetActive(true);

            currentVesselStats();
        }
    }

    string SwitchCamera()
    {
        if (vesselExists)
        {
            if (!birdCamOn)
            {
                instantiatedVessel.SetActive(false);
                BirdCam.SetActive(true);
                BirdXYZ.SetActive(true);
                activeCamera = BirdCam.transform.GetChild(0).gameObject;
                birdCamOn = true;
            }
            else
            {
                BirdCam.SetActive(false);
                instantiatedVessel.SetActive(true);
                BirdXYZ.SetActive(false);
                birdCamOn = false;
                activeCamera = instantiatedVessel.transform.GetChild(1).gameObject;
            }
            return "Camera Switched";
        }
        else
        {
            return "There is only one camera.";
        }
    }

    string GenerateVessel(string xyz)
    {
        if (!vesselExists)
        {
            String[] sptr = {","};
            String[] XYZ = xyz.Split(sptr, StringSplitOptions.RemoveEmptyEntries);
            float x = float.Parse(XYZ[0]);
            float y = float.Parse(XYZ[1]);
            float z = float.Parse(XYZ[2]);

            instantiatedVessel = Instantiate(vessel, new Vector3(x, y, z), transform.rotation);
            BirdCam.SetActive(false);
            birdCamOn = false;
            activeCamera = instantiatedVessel.transform.GetChild(1).gameObject;
            BirdXYZ.SetActive(false);
            instantiatedVessel.SetActive(true);
            vesselExists = true;
            return "Vessel generated";
        }
        else
            return "Vessel exists already";
    }

    string DestroyVessel()
    {
        if (vesselExists)
        {
            Destroy(instantiatedVessel);
            BirdCam.SetActive(true);
            BirdXYZ.SetActive(true);
            activeCamera = BirdCam.transform.GetChild(0).gameObject;
            vesselExists = false;
            return "Vessel destroyed";
        }
        else
            return "Vessel doesn't exist";
    }

    public void Display(String showText)
    {
        timeDisplayOut = true;
        displayTimeOut = 3f;
        displayText.GetComponent<Text>().text = showText;
        displayText.SetActive(true);
    }

    void GetTypedCommand(string arg0)
    {
        putInCommand = arg0;
    }

    void OnHelpButtonClicked()
    {
        helpPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEscClicked()
    {
        instantiatedVessel.transform.GetChild(1).GetComponent<MouseLook>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

        transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
    }
}
