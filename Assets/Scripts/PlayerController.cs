using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject sunLight;
    private Rigidbody playerRB;

    //###MAINCAMERA MUST ALSO FOLLOW PLAYER, AND MUST NOT ROTATE WITH THE PLAYER AS IT DOES SO
        //this can probably be done by getting player.localposition and offsetting

    private float horzInput;
    private float vertInput;
    private float movementMagnitude = 10f;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horzInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");

        //when on the ground during game's startup period, update local position.
            //###WHEN IN THE AIR, YOU NEED TO ADJUST RIGIDBODY VELOCITY INSTEAD
        gameObject.transform.localPosition += new Vector3(horzInput * movementMagnitude * Time.deltaTime,
            0f,
            vertInput * movementMagnitude * Time.deltaTime);

        //jump onSpace
        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(0f, 5000f, 0f, ForceMode.Impulse);
        }

        //no matter how player moves, sun must follow to ensure shadow remains square
        SunFollow();
    }
    void SunFollow()
    {
        sunLight.transform.position = new Vector3(gameObject.transform.position.x,
                sunLight.transform.position.y, gameObject.transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        //reset any existing velocity so it doesn't fight the new mine explosion
        playerRB.velocity = new Vector3(0f, 0f, 0f);

        //apply pregenerated forces to player
        other.gameObject.GetComponent<MineAbstract>().ApplyMineForce(playerRB);

        //increment score and destroy
        GameManager.instance.SetScorePlus(other.GetComponent<MineAbstract>().GetScoreValue());
        //###SDTZ
        print(GameManager.instance.GetScore());
        //###EDTZ
        Destroy(other.gameObject);
    }
}
