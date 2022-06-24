using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //required for scene transitions
using System.IO; //required for file IO

[DefaultExecutionOrder(1000)] //run only after the rest of the scene is initialized

public class Mast : MonoBehaviour
{
    public static Mast Er { get; private set; } //ENCAPSULATION (auto)

    private string playerName;

    //arrays are oversized by one element to allow for insertions without overflow error. End element is not
        //saved or loaded; it's just an overflow container.
    private string[] names;
    private int[] scores;
    private int currentPlaceBeingChecked;

    //~~~ENCAPSULATION
    public string GetPlayerName()
    {
        return playerName;
    }
    public void SetPlayerName(string name) //ENCAPSULATION (manual)
    {
        if (name == "" || name == null)
        {
            playerName = "Pvt. Null";
        }
        else
        {
            playerName = name;
        }
    }
    public string GetName(int index)
    {
        return names[index];
    }
    public int GetHighScore(int index)
    {
        return scores[index];
    }

    //~~~SINGLETON INSTANTIATION
    private void Awake()
    {
        names = new string[4];
        scores = new int[4];
        Er = this;
        DontDestroyOnLoad(gameObject);
    }

    //~~~SCENE MANAGEMENT
    private void Start()
    {
        ResetCurrentPlace();
        StartCoroutine(SceneLoadingToMenu());
        LoadData();
    }

    IEnumerator SceneLoadingToMenu()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
    public void SceneMenuToGame()
    {
        SceneManager.LoadScene(2);
    }
    public void SceneGameToMenu()
    {
        SaveData();
        SceneManager.LoadScene(1);
    }

    //~~~SCOREWORK
    public void CheckScore(int score)
    {
        if(currentPlaceBeingChecked == -1)
        {
            return;
        }
        if(score > scores[currentPlaceBeingChecked])
        {
            //###fire particle effect[i]
            currentPlaceBeingChecked--;
        }
    }
    public void RecordScore(int score)
    {
        //###call this on:
            //score reset - check
            //ReturnToMenu - check
        //before said call, ResetCurrentPlace(), as InsertScore is recursive

        if(score > scores[currentPlaceBeingChecked])
        {
            //if we beat this place, shift it downwards...
            names[currentPlaceBeingChecked + 1] = names[currentPlaceBeingChecked];
            scores[currentPlaceBeingChecked + 1] = scores[currentPlaceBeingChecked];
            
            //...then check the next place
            currentPlaceBeingChecked--;
            if(currentPlaceBeingChecked == -1)
            {
                //we beat first place! Since this is the recursive base-case, write the score instead of
                    //calling RecordScore() again.
                WriteScore(score);
            }
            else
            {
                RecordScore(score);
            }
        }
        else
        {
            WriteScore(score);
        }
    }
    private void WriteScore(int score)
    {
        //because of "then check next place", if score doesn't beat that place, the variable needs to be
            //incremented to go back to the last place we actually beat
        currentPlaceBeingChecked++;
        names[currentPlaceBeingChecked] = playerName;
        scores[currentPlaceBeingChecked] = score;
        SaveData();
    }
    public void ResetCurrentPlace()
    {
        //no need to check against 4th place (it might not even have data)
        currentPlaceBeingChecked = scores.Length - 2;
    }

    //~~~FILE IO
    [System.Serializable]//class definition requires this to be serialized into a json file
    class SData
    {
        public string playerName;
        public string[] names = new string[3];
        public int[] scores = new int[3];
    }
    public void SaveData() //ABSTRACTION (as if it's hard to find another example of that!) =D
    {
        //create new SaveData object and populate it with the current highscores
        SData data = new SData();

        data.playerName = playerName;
        for (int i = 0; i < data.names.Length; i++)
        {
            data.names[i] = names[i];
            data.scores[i] = scores[i];
        }

        //serialize that data into a JSON string
        string jsonData = JsonUtility.ToJson(data);

        //use file IO to write the json strong to a new json file.
        //Application.persistentDataPath autogenerates a folder in %localdata%. The exact path will
        //be different depending on the platform, but available in the Unity documentation.
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", jsonData);
    }
    public void LoadData()
    {
        //get filepath. This is hardcoded for obvious reasons, but can still be influenced by variables,
            //for example, in the form of rolling autosaves.
        string filepath = Application.persistentDataPath + "/savedata.json";

        //always check that the file exists in order to prevent crashes.
        if (File.Exists(filepath))
        {
            //import the json string...
            string jsonData = File.ReadAllText(filepath);
            //...and deserialize it into a new SaveData class object
            SData data = JsonUtility.FromJson<SData>(jsonData);

            //now that the data has been loaded, set Master's highscore data to whatever was saved.
            playerName = data.playerName;
            for (int i = 0; i < data.names.Length; i++)
            {
                names[i] = data.names[i];
                scores[i] = data.scores[i];
            }
        }
        else
        {
            //if there is no init save file, create one
            ResetData();
        }
    }
    public void ResetData()
    {
        playerName = "Pvt. Null";
        names[0] = "Goldie";
        names[1] = "Sylvie";
        names[2] = "Bronzie";
        scores[0] = 5;
        scores[1] = 3;
        scores[2] = 2;

        //by saving data immediately, this effectively overwrites whatever is there.
        SaveData();
    }
}
