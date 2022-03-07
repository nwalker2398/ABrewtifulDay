using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    static LevelController instance;
    List<Dictionary<string, object>> levels;

    [SerializeField] int currentLevel = -1;
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
        instance = this;
    }

    void getLevels()
    {
        levels = new List<Dictionary<string, object>>();
        int i = 0;
        foreach (string line in  File.ReadLines("Assets/Scripts/levels.csv"))
        {
            levels.Add(new Dictionary<string, object>());
            if (i == 0)
            {
                i++;
                continue;
            }
            string[] words = line.Split(',');
            levels[i]["Scene"] = words[1];
            levels[i]["CustomerSpeed"] = int.Parse(words[2]);
            levels[i]["NumWaves"] = int.Parse(words[3]);
            levels[i]["CustomersPerWave"] = int.Parse(words[4]);
            levels[i]["HeartQuota"] = int.Parse(words[5]);
            levels[i]["CoffeeEnabled"] = bool.Parse(words[6]);
            levels[i]["MatchaEnabled"] = bool.Parse(words[7]);
            levels[i]["BobaEnabled"] = bool.Parse(words[8]);
            levels[i]["Picture1Enabled"] = bool.Parse(words[9]);
            levels[i]["Plant1Enabled"] = bool.Parse(words[10]);
            levels[i]["WallPainted"] = bool.Parse(words[11]);
            levels[i]["WallChoices"] = bool.Parse(words[12]);
            levels[i]["Picture2Enabled"] = bool.Parse(words[13]);
            string[] locations = words[14].Split('|');
            levels[i]["CoffeeSpillLocation"] = new Vector3(int.Parse(locations[0]), int.Parse(locations[1]), int.Parse(locations[2]));
            levels[i]["Plant2Enabled"] = bool.Parse(words[15]);
            levels[i]["Dialog"] = words[16];
            i++;
        }
        Debug.Log("Loaded " + levels.Count + " levels.");
    }

    // Start is called before the first frame update
    void Start()
    {
        getLevels();
    }

    // Update is called once per frame
    void Update()
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

    void loadLevel(int level)
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
