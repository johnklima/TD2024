using UnityEngine;

public class CustomCharacterController : MonoBehaviour
{
    public float speed = 10.0f;
    private float strafe;
    private float translation;

    // Use this for initialization
    private void Start()
    {
        // turn off the cursor
        // Cursor.lockState = CursorLockMode.Locked;		
    }

    // Update is called once per frame
    private void Update()
    {
        // Input.GetAxis() is used to get the user's input
        // You can furthor set it on Unity. (Edit, Project Settings, Input)
        translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        strafe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(strafe, 0, translation);

        // if (Input.GetKeyDown("escape"))
        //     // turn on the cursor
        //     Cursor.lockState = CursorLockMode.None;
    }
}