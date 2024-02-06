using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronMinigame : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject pivotObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Cauldron minigame, aka stirring ladle
        transform.RotateAround(pivotObject.transform.position, new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime);
    }
}
