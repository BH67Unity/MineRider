using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    private float speed = -5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(new Vector3(0f, 0f, speed * Time.deltaTime));

        if(gameObject.transform.position.z < -30)
        {
            Destroy(gameObject);
        }
    }
}
