using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronMinigame : MonoBehaviour
{
    public float rotationSpeed;
    public float stirFrequency = 100;
    public Transform pivotObject;

    public float angle = 0; //this is in deegrees where radians
                            //(which are a fraction pi) are more scientific.
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Cauldron minigame, aka stirring ladle
        //transform.RotateAround(pivotObject.transform.position, new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime);
     


        float dir = Mathf.Sin(Time.time) * stirFrequency;
        //transform.Rotate(transform.up, Time.deltaTime * rotationSpeed * dir);


        transform.RotateAround( pivotObject.position, 
                                pivotObject.up, 
                                rotationSpeed * Time.deltaTime );


    }
}
