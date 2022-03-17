using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelController : MonoBehaviour
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
        string big_string = "1,Tutorial,10,0,1,5,TRUE,FALSE,FALSE,FALSE,FALSE,FALSE,FALSE,FALSE,0|0|0,FALSE,,0\n2,Cafe1,10,0,1,15,TRUE,FALSE,FALSE,FALSE,FALSE,FALSE,FALSE,FALSE,0|0|0,FALSE,,0\n3,Cafe1,10,1,2,18,TRUE,FALSE,FALSE,FALSE,FALSE,FALSE,FALSE,FALSE,0|0|0,FALSE,,0\n4,Cafe1,9,1,2,21,TRUE,FALSE,FALSE,FALSE,FALSE,FALSE,FALSE,FALSE,0|0|0,FALSE,I noticed your walls are looking a little bare^ so I got you this poster! Thanks for making such great coffee!,1\n5,Cafe1,9,2,2,24,TRUE,FALSE,FALSE,TRUE,FALSE,FALSE,FALSE,FALSE,0|0|0,FALSE,,0\n6,Cafe1,8,2,2,27,TRUE,FALSE,FALSE,TRUE,TRUE,FALSE,FALSE,FALSE,0|0|0,FALSE,You're doing such a great job! I grew these plants in my garden and I think they would look great in your shop if you want them!,0\n7,Cafe1,8,2,2,30,TRUE,FALSE,FALSE,TRUE,TRUE,FALSE,FALSE,FALSE,0|0|0,FALSE,,0\n8,Cafe1,8,3,3,40,TRUE,FALSE,FALSE,TRUE,TRUE,FALSE,FALSE,FALSE,0|0|0,FALSE,Did you hear? It's national COFFEE day!! How exciting^ I bet you'll have a ton of customers today!,0\n9,Cafe1,7,3,2,33,TRUE,FALSE,FALSE,TRUE,TRUE,TRUE,TRUE,FALSE,0|0|0,FALSE,Hey^ I have some extra paint. Each can is the perfect amount for your walls. If you pick one of these colors^ I can paint up the walls for you! Your coffee has helped me through all my hardest workdays ;),3\n10,Cafe2,7,2,3,25,TRUE,FALSE,FALSE,TRUE,TRUE,TRUE,FALSE,FALSE,0|0|0,FALSE,Wow^ your shop is busy these days. I'm a carpenter^ so let me make you a few more tables. As a thanks for making the best coffee in town!,0\n11,Cafe2,7,1,3,27,TRUE,TRUE,FALSE,TRUE,TRUE,TRUE,FALSE,FALSE,0|0|0,FALSE,Your coffee is soooo tasty! I think you could be really good at making matcha drinks too. Here^ have my machine^ I don't use it enough.,0\n12,Cafe2,7,3,2,29,TRUE,TRUE,FALSE,TRUE,TRUE,TRUE,FALSE,FALSE,0|0|0,FALSE,,0\n13,Cafe2,7,3,2,31,TRUE,TRUE,FALSE,TRUE,TRUE,TRUE,FALSE,TRUE,0|0|0,FALSE,This is my favorite cafe! I made this art for you and you can put it up if you want.,2\n14,Cafe2,6,2,2,33,TRUE,TRUE,FALSE,TRUE,TRUE,TRUE,FALSE,TRUE,12|0|46,FALSE,,0\n15,Cafe3,6,1,4,30,TRUE,TRUE,FALSE,TRUE,TRUE,TRUE,FALSE,TRUE,0|0|0,FALSE,Hello I see that the last two tables I made for you are getting a lot of use! Let me make you a couple more. Keep up the good work!,0\n16,Cafe3,6,2,2,31,TRUE,TRUE,TRUE,TRUE,TRUE,TRUE,FALSE,TRUE,0|0|0,FALSE,Have you heard of this amaaaazing drink called Boba? I think you would be good at making it! Here's a boba machine to help you get started!,0\n17,Cafe3,6,3,3,32,TRUE,TRUE,TRUE,TRUE,TRUE,TRUE,FALSE,TRUE,0|0|0,FALSE,,0\n18,Cafe3,6,3,2,33,TRUE,TRUE,TRUE,TRUE,TRUE,TRUE,FALSE,TRUE,0|0|0,TRUE,Hi! I saw that you like plants and I have this fiddle leaf fig tree that I can't take care of anymore. I want you to have it!,4\n19,Cafe3,6,2,2,34,TRUE,TRUE,TRUE,TRUE,TRUE,TRUE,FALSE,TRUE,0|0|0,TRUE,,0\n20,Cafe3,6,2,3,35,TRUE,TRUE,TRUE,TRUE,TRUE,TRUE,FALSE,TRUE,25|0|30,TRUE,,0\n21,Cafe3,6,3,4,41,TRUE,TRUE,TRUE,TRUE,TRUE,TRUE,FALSE,TRUE,0|0|0,TRUE,Guess what?! It's national DRINK day! I hope it goes well for you today^ all your drinks are so tasty!,0";
        TextAsset level_csv_ta = Resources.Load<TextAsset>("levels");
        Debug.Log(level_csv_ta);
        foreach (string line in level_csv_ta.text.Split('\n')) //File.ReadLines("Assets/Resources/levels.csv")
        {
            if (i == 0)
            {
                i++;
                continue;
            }
            Dictionary<string, object> level = new Dictionary<string, object>();
            string[] words = line.Split(',');
            Debug.Log(string.Join(",", words));
            if (words.Length > 1)
            {
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
                level["Placing"] = int.Parse(words[17]);

                levels.Add(level);
            }
        }
        Debug.Log("Loaded " + levels.Count + " levels.");
        Debug.Log(big_string);
    }

    public void getSongs()
    {
        songs = new List<AudioClip>();
        songs.Add(null);
        for (int i = 1; i < levels.Count; i++)
        {
            songs.Add(Resources.Load<AudioClip>("Music/Song" + i));
            // Debug.Log(songs[i].name);
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
        foreach (KeyValuePair<string, object> kvp in level)
        {
            Debug.Log(string.Format("{0}={1}", kvp.Key, kvp.Value));
        }
    }

    public void Update()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("MainCamera"))
        {
            if (o.GetComponent<AudioSource>().clip.name != songs[currentLevel].name)
            {
                if ((int)levels[currentLevel]["Placing"] == 0)
                {
                    Debug.Log("update function, placing is 0");
                    loadLevel();
                }
                o.GetComponent<AudioSource>().Play();
            }
        }

    }

    public static void toggleUnlimitedHearts()
    {
        LC.unlimitedHearts = !LC.unlimitedHearts;
        Debug.Log("Unlimited hearts = " + LC.unlimitedHearts);
        if (LC.unlimitedHearts)
            ScoreSystem.setScore(ScoreSystem.getMaxScore());
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
        // check for if we need to go to editing scene
        if ((int)levels[currentLevel]["Placing"] != 0)
        {
            Debug.Log("gets to here. level[placing] = " + levels[currentLevel]["Placing"]);
            // do the dialog box stuff
            // go to editing mode scene
            SceneManager.LoadScene("EditingMode");
        }
        else
        {
            // else call these next two lines
            SceneManager.LoadScene("" + levels[currentLevel]["Scene"]);
            loadLevel();
        }
    }

    public void loadLevelAfterEdit()
    {
        Debug.Log("Open shop Button clicked");
        SceneManager.LoadScene("" + levels[currentLevel]["Scene"]);
        Debug.Log("Next scene loaded");
        loadLevel();
    }

    public int getPlacing()
    {
        return (int)levels[currentLevel]["Placing"];
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
            if (o.scene.name != "" + levels[currentLevel]["Scene"])
            {
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
        if (!spillPos.Equals(new Vector3(0, 0, 0)))
            Instantiate(spillPrefab, spillPos, Quaternion.Euler(0, 90, 0));
        ScoreSystem.setMaxScore((int)levels[level]["HeartQuota"]);
        ScoreSystem.setScore(0);
        GameObject progressBar = GameObject.FindGameObjectWithTag("ProgressBar");
        ProgressBar pb = progressBar.GetComponent<ProgressBar>();
        print(pb);
        pb.SetMaxProgress((int)levels[level]["HeartQuota"]);
        pb.SetInitialProgress(0);
        if (unlimitedHearts)
            ScoreSystem.setScore(ScoreSystem.getMaxScore());

        GameController.GC.resumeGame();
        Debug.Log("gets to end of load level");
    }
}
