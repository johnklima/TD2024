using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    public float moveSpeed = 10;
    public float turnSpeed = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float turn = Input.GetAxis("Horizontal");
        float move = Input.GetAxis("Vertical");

        turn *= turnSpeed * Time.deltaTime;
        move *= moveSpeed * Time.deltaTime;

        transform.Rotate(0, turn, 0);
        transform.position += transform.forward * move;




    }
}
