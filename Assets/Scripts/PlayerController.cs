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
    private float transformMagnitude = 10f;
    private float velocityMagnitude = 3f;
    private float eJumpForce = 5000;

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
        gameObject.transform.localPosition += new Vector3(horzInput * transformMagnitude * Time.deltaTime,
            0f,
            vertInput * transformMagnitude * Time.deltaTime);
        playerRB.velocity += new Vector3(horzInput * velocityMagnitude * Time.deltaTime,
            0,
            vertInput * velocityMagnitude * Time.deltaTime);

        //onSpace, use an E-Jump manually if available
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UseEJump(false);
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
        other.gameObject.GetComponent<MineAbstract>().CollisionExtras();

        //increment score and check
        GameManager.instance.SetScorePlus(other.GetComponent<MineAbstract>().GetScoreValue());
        Mast.Er.CheckScore(GameManager.instance.GetScore());
        //destroy mine
        Destroy(other.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            UseEJump(true);
        }
    }
    private void UseEJump(bool onGround)
    {
        if(GameManager.instance.SetEJumpsDecrement())
        {
            playerRB.AddForce(0, eJumpForce, 0, ForceMode.Impulse);
        }
        else
        {
            if(onGround)
            {
                Mast.Er.ResetCurrentPlace();
                Mast.Er.RecordScore(GameManager.instance.GetScore());
                GameManager.instance.ResetEJumps();
                GameManager.instance.ResetScore();

                //reset player
                gameObject.transform.rotation = Quaternion.identity;
                playerRB.velocity = new Vector3(0f, 0f, 0f);
                playerRB.angularVelocity = new Vector3(0f, 0f, 0f);
                gameObject.transform.position = new Vector3(0f, 11.5f, -24f);
                GameManager.instance.resetSpawnPlatform();
            }
        }
    }
}
