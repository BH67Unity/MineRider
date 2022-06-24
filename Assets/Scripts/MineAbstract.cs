using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MineAbstract : MonoBehaviour //INHERITANCE (see the other 5 mine classes)
{
    protected float moveSpeed = -10;
    protected int scoreValue;
    protected float rX, rY, rZ;

    public int GetScoreValue()
    {
        return scoreValue;
    }

    public abstract void GenMineForce(); //POLYMORPHISM (see the other 5 mine classes
    public abstract void CollisionExtras();

    protected virtual void Start()
    {
        //normally it would be more efficient to calculate this onCollide rather than at mineGen, simply because
            //the player is never going to collide with every mine that's generated. For the purposes of using
            //OOP principles, it will be done here.
        GenMineForce();
    }
    protected virtual void Update()
    {
        MoveDown();
    }
    protected void MoveDown()
    {
        gameObject.transform.Translate(new Vector3(0f, 0f, moveSpeed * Time.deltaTime));

        if (gameObject.transform.position.z < -30)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyMineForce(Rigidbody playerRB)
    {
        playerRB.AddForce(rX, rY, rZ, ForceMode.Impulse);
        //###do the same thing for torque
    }
}
