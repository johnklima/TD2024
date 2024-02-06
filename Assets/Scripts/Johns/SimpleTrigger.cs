using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class SimpleTrigger : MonoBehaviour
{
    
    bool isInTrigger = false;
    SimpleCamera cam;
    float camdist;
    float camheight;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<SimpleCamera>();
        camheight = cam.height;
        camdist = cam.distance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInTrigger)
        {
            Debug.Log("Player pressed E key");
            
            //zoom in camera
            cam.distance = 3;
            cam.height = 4;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log(other.name + " triggered enter " + transform.name);
            //Do something...
            isInTrigger = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(other.name + " triggered stay " + transform.name);
            //Do something...
            isInTrigger = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(other.name + " triggered exit " + transform.name);
            //Do something...
            isInTrigger = false;
            //restore camera
            cam.distance = camdist;
            cam.height = camheight;

        }
    }
}
