using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelController: MonoBehaviour
{
    public static LevelController LC;
    public List<Dictionary<string, object>> levels;
    public int currentLevel = 0;
    public Dictionary<string, object> currentLevelData;

    // [SerializeField] int currentLevel = -1;
    public GameObject coffeeMachine;
    public GameObject matchaMachine;
    public GameObject bobaMachine;
    public GameObject picture1;
    public GameObject plant1;
    public GameObject walls;
    public GameObject picture2;
    public GameObject plant2;
    public GameObject camera;

    private int oldLevel = -1;

    void Awake()
    {
        if (LC != null)
        {
            GameObject.Destroy(this);
        }
        else
        {
            LC = this;
        }
        DontDestroyOnLoad(this);
        StartLevels();
    }

    void getLevels()
    {
        levels = new List<Dictionary<string, object>>();
        int i = 0;
        foreach (string line in  File.ReadLines("Assets/Scripts/levels.csv"))
        {
            if (i == 0)
            {
                i++;
                continue;
            }
            Dictionary<string, dynamic> level = new Dictionary<string, dynamic>();
            string[] words = line.Split(',');
            level["Scene"] = words[1];
            level["GenerateCustomerIn"] = float.Parse(words[2]);
            level["NumWaves"] = int.Parse(words[3]);
            level["CustomersPerWave"] = int.Parse(words[4]);
            level["HeartQuota"] = int.Parse(words[5]);
            level["CoffeeEnabled"] = bool.Parse(words[6]);
            level["MatchaEnabled"] = bool.Parse(words[7]);
            level["BobaEnables"] = bool.Parse(words[8]);
            level["Picture1Enabled"] = bool.Parse(words[9]);
            level["Plant1Enabled"] = bool.Parse(words[10]);
            level["WallPainted"] = bool.Parse(words[11]);
            level["WallChoices"] = bool.Parse(words[12]);
            level["Picture2Enabled"] = bool.Parse(words[13]);
            string[] locations = words[14].Split('|');
            level["CoffeeSpillLocation"] = new Vector3(int.Parse(locations[0]), int.Parse(locations[1]), int.Parse(locations[2]));
            level["Plant2Enabled"] = bool.Parse(words[15]);
            level["Dialog"] = words.Length > 16 ? words[16] : "";

            levels.Add(level);
        }
        Debug.Log("Loaded " + levels.Count + " levels.");
    }

    // Call this once when the game first loads
    public void StartLevels()
    {
        getLevels();
        currentLevelData = levels[currentLevel];
    }

    public void printLevel(int levelNumber)
    {
        Dictionary<string, object> level = levels[levelNumber];
        foreach(KeyValuePair<string, object> kvp in level)
        {
            Debug.Log(string.Format("{0}={1}", kvp.Key, kvp.Value));
        }
    }

    public void Update()
    {
        if(currentLevel > 0 && (!currentLevel.Equals(oldLevel)))
        {
            setLevel(currentLevel);
            oldLevel = currentLevel;
        }
    }

    public void setLevel(int l)
    {
        Debug.Log("Setting level to " + l);
        currentLevel = l;
        loadLevel(currentLevel);
        oldLevel = currentLevel;
    }

    public void nextLevel()
    {
        Debug.Log("Next Level: " + (currentLevel + 1));
        currentLevel++;
        loadLevel(currentLevel);
        oldLevel = currentLevel;
    }

    public void loadLevel(int level)
    {
        SceneManager.LoadScene("" + levels[level]["Scene"]);
        if(coffeeMachine)
            coffeeMachine.SetActive((bool)levels[level]["CoffeeEnabled"]);
        if(matchaMachine)
            matchaMachine.SetActive((bool)levels[level]["MatchaEnabled"]);
        if (bobaMachine)
            bobaMachine.SetActive((bool)levels[level]["BobaEnabled"]);
        if (picture1)
            picture1.SetActive((bool)levels[level]["Picture1Enabled"]);
        if (plant1)
            plant1.SetActive((bool)levels[level]["Plant1Enabled"]);
        //if(levels[level]["WallPainted"])
        //    walls.color = levels[level]["WallColor"];
        if (picture2)
            picture2.SetActive((bool)levels[level]["Picture2Enabled"]);
        if (plant2)
            plant2.SetActive((bool)levels[level]["Plant2Enabled"]);

        if (camera)
            camera.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Assets/Audio/Song" + level + ".wav");
    }
}
