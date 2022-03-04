using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelController : MonoBehaviour
{
    static LevelController instance;
    List<Dictionary<string, dynamic>> levels;

    void Awake()
    {
        instance = this;
    }

    void getLevels()
    {
        levels = new List<Dictionary<string, dynamic>>();
        int i = 0;
        foreach (string line in  File.ReadLines("levels.csv"))
        {
            levels.Add(new Dictionary<string, dynamic>());
            string[] words = line.Split(',');
            levels[i]["Scene"] = words[1];
            levels[i]["CustomerSpeed"] = int.Parse(words[2]);
            levels[i]["NumWaves"] = int.Parse(words[3]);
            levels[i]["CustomersPerWave"] = int.Parse(words[4]);
            levels[i]["HeartQuota"] = int.Parse(words[5]);
            levels[i]["CoffeeEnabled"] = bool.Parse(words[6]);
            levels[i]["MatchaEnabled"] = bool.Parse(words[7]);
            levels[i]["BobaEnables"] = bool.Parse(words[8]);
            levels[i]["Picture1Enabled"] = bool.Parse(words[9]);
            levels[i]["Plant1Enabled"] = bool.Parse(words[10]);
            levels[i]["WallPainted"] = bool.Parse(words[11]);
            levels[i]["WallChoices"] = bool.Parse(words[12]);
            levels[i]["Picture2Enabled"] = bool.Parse(words[13]);
            string[] locations = words[14].Split('|');
            levels[i]["CoffeeSpillLocation"] = new Vector3(int.Parse(locations[0]), int.Parse(locations[1]), int.Parse(locations[2]));
            levels[i]["Plant2Enabled"] = bool.Parse(words[15]);
            levels[i]["Dialog"] = words[16];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        getLevels();
        Debug.Log("asdfadsgasdhha");
        Debug.Log("asdfadsgasdhha");
        Debug.Log("asdfadsgasdhha");
        Debug.Log("asdfadsgasdhha");
        Debug.Log("asdfadsgasdhha");
        Debug.Log("asdfadsgasdhha");
        Debug.Log("asdfadsgasdhha");
        Debug.Log("asdfadsgasdhha");
        Debug.Log("asdfadsgasdhha");
        Debug.Log("asdfadsgasdhha");
        Debug.Log("asdfadsgasdhha");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
