using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject minePrefab;
    public GameObject superMinePrefab;
    public GameObject eMinePrefab;
    
    private float spawnIntervalCounter;
    private float rX, rY, rZ;
    private int rnd;
    // Start is called before the first frame update
    void Start()
    {
        spawnIntervalCounter = 2;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCounter();
    }
    void UpdateCounter()
    {
        spawnIntervalCounter -= Time.deltaTime;
        if(spawnIntervalCounter <= 0)
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
