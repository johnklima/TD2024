using UnityEngine;

public class SimpleTrigger : MonoBehaviour
{
    private SimpleCamera cam;
    private float camdist;
    private float camheight;

    private bool isInTrigger;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main.GetComponent<SimpleCamera>();
        camheight = cam.height;
        camdist = cam.distance;
    }

    // Update is called once per frame
    private void Update()
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
        if (other.tag == "Player")
        {
            Debug.Log(other.name + " triggered enter " + transform.name);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(other.name + " triggered stay " + transform.name);
            //Do something...
            isInTrigger = true;
        }
    }
}