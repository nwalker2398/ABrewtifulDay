using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelController : MonoBehaviour
{
    static LevelController instance;
    List<Dictionary<string, object>> levels;

    [SerializeField] int currentLevel;
    public GameObject coffeeMachine;
    public GameObject matchaMachine;
    public GameObject bobaMachine;
    public GameObject picture1;
    public GameObject plant1;
    public GameObject walls;
    public GameObject picture2;
    public GameObject plant2;
    public GameObject camera;

    private int oldLevel;

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
            if (i == 0)
            {
                i++;
                continue;
            }
            levels.Add(new Dictionary<string, object>());
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
    }

    // Start is called before the first frame update
    void Start()
    {
        getLevels();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void loadLevel(int level)
    {
        coffeeMachine.active = (bool)levels[level]["CoffeeEnabled"];
        matchaMachine.active = (bool)levels[level]["MatchaEnabled"];
        bobaMachine.active = (bool)levels[level]["BobaEnabled"];
        picture1.active = (bool)levels[level]["Picture1Enabled"];
        plant1.active = (bool)levels[level]["Plant1Enabled"];
        //if(levels[level]["WallPainted"])
        //    walls.active = levels[level]["CoffeeEnabled"];
        picture2.active = (bool)levels[level]["Picture2Enabled"];
        plant2.active = (bool)levels[level]["Plant2Enabled"];

        camera.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Assets/Audio/Song" + level + ".wav");
    }
}
