using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //required for UI

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] GameObject eJumpsText;
    [SerializeField] GameObject scoreText;
    [SerializeField] GameObject returnToMenuButton;

    [SerializeField] GameObject spawnPlatform;

    [SerializeField] GameObject minePrefab;
    [SerializeField] GameObject superMinePrefab;
    [SerializeField] GameObject eMinePrefab;

    private float spawnIntervalCounter;
    private float rX, rY, rZ;
    private int rnd;

    private int score = 0;
    private int eJumps = 3;

    public int GetScore()
    {
        return score;
    }
    public void ResetScore()
    {
        //reset score
        score = 0;
        UpdateScoreText();

        //reset internal currentPlace, which is used to determine where score fits in highscores list
        Mast.Er.ResetCurrentPlace();
    }
    public void SetScorePlus(int add)
    {
        //score should only be added to. Resetting score should be done with ResetScore()
        if (add < 0)
        {
            add *= -1;
        }
        score += add;
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }
    public int GetEJumps()
    {
        return eJumps;
    }
    public void SetEJumpsIncrement()
    {
        eJumps++;
        UpdateEJumpsText();
    }
    public bool SetEJumpsDecrement()
    {
        if(eJumps > 0) //ejumps remaining
        {
            eJumps--;
            UpdateEJumpsText();
            return true;
        }
        else //no ejumps remaining - therefore, ejump functionality will not happen!
        {
            return false;
        }
    }
    public void ResetEJumps()
    {
        eJumps = 3;
        UpdateEJumpsText();
    }
    private void UpdateEJumpsText()
    {
        eJumpsText.GetComponent<TextMeshProUGUI>().text = "E-Jumps: " + eJumps;
    }
    private void Awake()
    {
        spawnIntervalCounter = 2;
        instance = this;
    }
    
    //~~~FUNCTIONALITY
    // Update is called once per frame
    void Update()
    {
        UpdateCounter();
    }
    void UpdateCounter()
    {
        spawnIntervalCounter -= Time.deltaTime;
        if (spawnIntervalCounter <= 0)
        {
            SpawnMine();
        }
    }
    void SpawnMine()
    {
        rX = Random.Range(-23f, 23f);
        rZ = 50f;
        rnd = Random.Range(1, 101);

        if (rnd < 91)
        {
            rY = 0.75f;
            Instantiate(minePrefab, new Vector3(rX, rY, rZ), minePrefab.transform.rotation);
        }
        else if (rnd < 96)
        {
            rY = 1;
            Instantiate(superMinePrefab, new Vector3(rX, rY, rZ), minePrefab.transform.rotation);
        }
        else
        {
            rY = 1;
            Instantiate(eMinePrefab, new Vector3(rX, rY, rZ), minePrefab.transform.rotation);
        }

        spawnIntervalCounter = Random.Range(0.5f, 1f);
    }
    public void resetSpawnPlatform()
    {
        spawnPlatform.SetActive(true);
    }
    public void ReturnToMenuClicked()
    {
        Mast.Er.ResetCurrentPlace();
        Mast.Er.RecordScore(score);
        Mast.Er.SceneGameToMenu();
    }
}
