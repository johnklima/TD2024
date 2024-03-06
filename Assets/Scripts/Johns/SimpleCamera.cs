using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float distance;
    public float height;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position 
                            - (target.forward * distance) 
                            + (target.up * height);


        transform.LookAt(target.position);

        if (Input.anyKey)
        {
            Debug.Log("KEY PRESSED");
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKey(kcode))
                    Debug.Log("key press " +  kcode.ToString() );
            }
        }
        if (Input.GetAxis("Jump") != 0)
            Debug.Log("jump");

        if ( Input.GetAxis("Vertical") != 0)
            Debug.Log("vertical");

        if (Input.GetAxis("Horizontal") != 0)
            Debug.Log("horizontal");



    }
}
