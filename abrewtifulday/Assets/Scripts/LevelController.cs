using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelController: MonoBehaviour
{
    public static LevelController LC;
    public List<Dictionary<string, object>> levels;
    public int currentLevel = 0;
    public Dictionary<string, object> currentLevelData;
    public List<AudioClip> songs;
    public GameObject spillPrefab;
    public bool unlimitedHearts = false;

    private int oldLevel;

    void Awake()
    {
        if (LC != null)
        {
            GameObject.Destroy(this);
            this.enabled = false;
            return;
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
        levels.Add(new Dictionary<string, object>());
        foreach (string line in  File.ReadLines("Assets/Scripts/levels.csv"))
        {
            if (i == 0)
            {
                i++;
                continue;
            }
            Dictionary<string, object> level = new Dictionary<string, object>();
            string[] words = line.Split(',');
            level["Scene"] = words[1];
            level["GenerateCustomerIn"] = float.Parse(words[2]);
            level["NumWaves"] = int.Parse(words[3]);
            level["CustomersPerWave"] = int.Parse(words[4]);
            level["HeartQuota"] = int.Parse(words[5]);
            level["CoffeeEnabled"] = bool.Parse(words[6]);
            level["MatchaEnabled"] = bool.Parse(words[7]);
            level["BobaEnabled"] = bool.Parse(words[8]);
            level["Picture1Enabled"] = bool.Parse(words[9]);
            level["Plant1Enabled"] = bool.Parse(words[10]);
            level["WallPainted"] = bool.Parse(words[11]);
            level["WallChoices"] = bool.Parse(words[12]);
            level["Picture2Enabled"] = bool.Parse(words[13]);
            string[] locations = words[14].Split('|');
            level["CoffeeSpillLocation"] = new Vector3(float.Parse(locations[0]), float.Parse(locations[1]), float.Parse(locations[2])); ;
            level["Plant2Enabled"] = bool.Parse(words[15]);
            level["Dialog"] = words.Length > 16 ? words[16].Replace("^", ",") : "";

            levels.Add(level);
        }
        Debug.Log("Loaded " + levels.Count + " levels.");
    }

    public void getSongs()
    {
        songs = new List<AudioClip>();
        songs.Add(null);
        for(int i = 1; i < levels.Count ; i++)
        {
            songs.Add(Resources.Load<AudioClip>("Music/Song" + i));
            Debug.Log(songs[i].name);
        }
    }

    // Call this once when the game first loads
    public void StartLevels()
    {
        oldLevel = -1;
        getLevels();
        getSongs();
        currentLevelData = levels[currentLevel];
        loadLevel();
        Debug.Log("Loading level!");
        Debug.Log(currentLevel);
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
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("MainCamera")){
            if (o.GetComponent<AudioSource>().clip.name != songs[currentLevel].name)
            {
                loadLevel();
                o.GetComponent<AudioSource>().Play();
            }
        }
        
    }

    public static void toggleUnlimitedHearts()
    {
        LC.unlimitedHearts = !LC.unlimitedHearts;
        Debug.Log("Unlimited hearts = " + LC.unlimitedHearts);
    }

    void setLevel(int l)
    {
        Debug.Log("Setting level to " + l);
        currentLevel = l;
        loadLevel();
        oldLevel = currentLevel;
    }

    public static void level1()
    {
        if (LC != null)
            LC.setLevel(1);
    }
    public static void level2()
    {
        if (LC != null)
            LC.setLevel(2);
    }

    public static void nextLevel()
    {
        if (LC != null)
            LC.instanceNextLevel();
    }

    public void instanceNextLevel()
    {
        Debug.Log("Next Level: " + (currentLevel + 1));
        currentLevel++;
        oldLevel = currentLevel;
        SceneManager.LoadScene("" + levels[currentLevel]["Scene"]);
        loadLevel();
    }

    public static void mainMenu()
    {
        Debug.Log("Main Menu");
        SceneManager.LoadScene("StartSceneBasic");
        LC.die();
    }

    public void die()
    {
        GameObject.Destroy(this);
        this.enabled = false;
        return;
    }

    public void loadLevel()
    {
        int level = currentLevel;

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("UI"))
            o.GetComponentInChildren<TextMeshProUGUI>().text = "Day " + level;

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("MainCamera"))
        {
            if(o.scene.name != "" + levels[currentLevel]["Scene"]){
                SceneManager.LoadScene("" + levels[currentLevel]["Scene"]);
            }

            o.GetComponent<AudioSource>().clip = songs[level];
            o.GetComponent<AudioSource>().Play();
        }

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("CoffeeStation"))
            o.SetActive((bool)levels[level]["CoffeeEnabled"]);
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("MatchaStation"))
            o.SetActive((bool)levels[level]["MatchaEnabled"]);
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("BobaStation"))
            o.SetActive((bool)levels[level]["BobaEnabled"]);
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Picture1"))
            o.SetActive((bool)levels[level]["Picture1Enabled"]);
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Plant1"))
            o.SetActive((bool)levels[level]["Plant1Enabled"]);
        //if(levels[level]["WallPainted"])
        //    walls.color = levels[level]["WallColor"];
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Picture2"))
            o.SetActive((bool)levels[level]["Picture2Enabled"]);
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Plant2"))
            o.SetActive((bool)levels[level]["Plant2Enabled"]);
        Vector3 spillPos = (Vector3)levels[level]["CoffeeSpillLocation"];
        if(!spillPos.Equals(new Vector3(0,0,0)))
            Instantiate(spillPrefab, spillPos, Quaternion.Euler(0, 90, 0));
    }
}
