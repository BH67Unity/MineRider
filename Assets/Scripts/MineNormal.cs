using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineNormal : MineAbstract
{
    protected override void Start()
    {
        scoreValue = 1;
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
        //no extras
    }
}
