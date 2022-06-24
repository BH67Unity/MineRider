using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineEJump : MineAbstract
{
    // Start is called before the first frame update
    protected override void Start()
    {
        scoreValue = 3;
        moveSpeed = -20;
        base.Start();
    }
    public override void GenMineForce()
    {
        rX = Random.Range(-1000f, 1000f);
        rY = Random.Range(3000f, 5000f);
        rZ = Random.Range(-1000f, 1000f);
    }
    public override void CollisionExtras()
    {
        GameManager.instance.SetEJumpsIncrement();
    }
}
