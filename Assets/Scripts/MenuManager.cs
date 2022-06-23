using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;//required for TMP UI interactions
using UnityEditor; //required for conditional compiling based on editor vs. build

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TMP_InputField playerNameInput;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject resetHighScoresButton;
    [SerializeField] GameObject quitButton;

    public void StartClicked()
    {
        Mast.Er.SceneMenuToGame();
    }
    public void ResetHighScoresClicked()
    {
        Mast.Er.ResetData();
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
