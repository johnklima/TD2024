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

    }
}
