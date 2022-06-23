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
    private float movementMagnitude = 5f;
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
        //if collide with mine, generate a randomize blast force and blast player upwards
        GenPlayerMineBlast(other.tag);
        Destroy(other.gameObject);
    }
    void GenPlayerMineBlast(string tag)
    {
        //zero out any pre-existing velocity transforms. This also ensures each blast doesn't fight against
            //the player's own downward velocity, therefore making the blast strength more reliable
        playerRB.velocity = new Vector3(0f, 0f, 0f);

        //main idea is that special mines will have their own, more powerful set of arguments, including greater
            //weight toward earning e-jumps.
        float rX, rY, rZ;
        rX = Random.Range(-1000f, 1000f);
        rY = Random.Range(3000f, 5000f);
        rZ = Random.Range(-1000f, 1000f);

        //###handle this in the child mine script so that you don't need a case statement
        //@@@mineCombo++;
        if (tag == "Super Mine")
        {
            rY *= 2;
            //@@@mineCombo += 2;
        }
        else if (tag == "E-Mine")
        {
            //@@@eJumps++;
            //@@@mineCombo += 4;
        }
        //###ADD RANDOM ANGULAR VELOCITIES TOO, AS WELL AS GRADUAL REDUCTION IN ROTATION WHILE AIRBORNE (ANGULAR DRAG?)

        gameObject.GetComponent<Rigidbody>().AddForce(rX, rY, rZ, ForceMode.Impulse);
    }
}
