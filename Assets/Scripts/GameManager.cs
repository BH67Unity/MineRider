using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public GameObject minePrefab;
    public GameObject superMinePrefab;
    public GameObject eMinePrefab;

    private float spawnIntervalCounter;
    private float rX, rY, rZ;
    private int rnd;

    private int score;

    public int GetScore()
    {
        return score;
    }
    public void SetScore0()
    {
        score = 0;
    }
    public void SetScorePlus(int add)
    {
        //score should only be added to. Resetting score should be done with SetScore0
        if (add < 0)
        {
            add *= -1;
        }
        score += add;
    }

    private void Awake()
    {
        spawnIntervalCounter = 2;
        instance = this;
    }

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
}
