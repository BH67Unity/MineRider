using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSuper : MineAbstract
{
    // Start is called before the first frame update
    protected override void Start()
    {
        scoreValue = 2;
        moveSpeed = -20;
        base.Start();
    }
    public override void GenMineForce()
    {
        rX = Random.Range(-2000f, 2000f);
        rY = Random.Range(6000f, 10000f);
        rZ = Random.Range(-2000f, 2000f);
    }
    public override void CollisionExtras()
    {
        //no extras
    }
}
