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
    public string name1, name2, name3;
    public int score1, score2, score3;

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

    private void Awake()
    {
        Er = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(SceneLoadingToMenu());
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
        SceneManager.LoadScene(1);
    }

    //~~~FILE IO
    [System.Serializable]//class definition requires this to be serialized into a json file
    class SData
    {
        public string playerName;
        public string name1, name2, name3;
        public int score1, score2, score3;
    }
    public void SaveData() //ABSTRACTION (as if it's hard to find another example of that!) =D
    {
        //create new SaveData object and populate it with the current highscores
        SData data = new SData();

        data.playerName = playerName;
        data.name1 = name1;
        data.name2 = name2;
        data.name3 = name3;
        data.score1 = score1;
        data.score2 = score2;
        data.score3 = score3;

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
            name1 = data.name1;
            name2 = data.name2;
            name3 = data.name3;
            score1 = data.score1;
            score2 = data.score2;
            score3 = data.score3;
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
        name1 = "Goldie";
        name2 = "Sylvie";
        name3 = "Bronzie";
        score1 = 100;
        score2 = 50;
        score3 = 30;

        //by saving data immediately, this effectively overwrites whatever is there.
        SaveData();
    }
}
