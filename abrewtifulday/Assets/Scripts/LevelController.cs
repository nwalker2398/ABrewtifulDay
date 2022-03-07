using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LevelController
{
    public static List<Dictionary<string, object>> levels;
    public static int currentLevel = 0;
    public static Dictionary<string, object> currentLevelData;

    static void getLevels()
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
    }

    // Call this once when the game first loads
    public static void StartLevels()
    {
        getLevels();
        currentLevelData = levels[currentLevel];
    }

    public static void printLevel(int levelNumber)
    {
        Dictionary<string, object> level = levels[levelNumber];
        foreach(KeyValuePair<string, object> kvp in level)
        {
            Debug.Log(string.Format("{0}={1}", kvp.Key, kvp.Value));
        }
    }
}
