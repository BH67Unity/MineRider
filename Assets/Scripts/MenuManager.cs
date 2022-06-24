using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;//required for TMP UI interactions
using UnityEditor; //required for conditional compiling based on editor vs. build

[DefaultExecutionOrder(1000)]
public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TMP_InputField playerNameInput;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject resetHighScoresButton;
    [SerializeField] GameObject quitButton;

    [SerializeField] GameObject name1Text;
    [SerializeField] GameObject name2Text;
    [SerializeField] GameObject name3Text;
    [SerializeField] GameObject score1Text;
    [SerializeField] GameObject score2Text;
    [SerializeField] GameObject score3Text;

    public void PopulateUI()
    {
        playerNameInput.text = Mast.Er.GetPlayerName();
        name1Text.GetComponent<TextMeshProUGUI>().text = Mast.Er.GetName(0);
        name2Text.GetComponent<TextMeshProUGUI>().text = Mast.Er.GetName(1);
        name3Text.GetComponent<TextMeshProUGUI>().text = Mast.Er.GetName(2);
        score1Text.GetComponent<TextMeshProUGUI>().text = Mast.Er.GetHighScore(0).ToString();
        score2Text.GetComponent<TextMeshProUGUI>().text = Mast.Er.GetHighScore(1).ToString();
        score3Text.GetComponent<TextMeshProUGUI>().text = Mast.Er.GetHighScore(2).ToString();
    }
    public void Start()
    {
        PopulateUI();
    }
    public void StartClicked()
    {
        //savedata is called purely to save the player name into the file. This way, input field
            //can be populated from save file unconditionally, rather than messing with singletons
            //and breakable menu item references. I'm looking at you, Scene Flow mission checkpoint!
        Mast.Er.SaveData();
        Mast.Er.SceneMenuToGame();
    }
    public void ResetHighScoresClicked()
    {
        Mast.Er.ResetData();
        PopulateUI();
    }
    public void QuitClicked()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    public void playerNameInputTextChanged()
    {
        Mast.Er.SetPlayerName(playerNameInput.text);
    }
}
