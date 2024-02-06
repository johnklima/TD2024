using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirPoint : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float dir = Mathf.Sin(Time.time) * 50;
        transform.Rotate(transform.up, Time.deltaTime * 5 * dir);
    }
}
