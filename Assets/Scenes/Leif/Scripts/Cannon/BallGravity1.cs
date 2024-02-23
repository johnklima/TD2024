using UnityEngine;

public class BallGravity1 : MonoBehaviour
{
    //gravity in meters per second per second
    public float GRAVITY_CONSTANT = -9.8f; // -- for earth,  -1.6 for moon 

    public Vector3 velocity = new(0, 0, 0); //current direction and speed of movement
    public Vector3 acceleration = new(0, 0, 0); //movement controlled by player movement force and gravity

    public Vector3 thrust = new(0, 0, 0); //player applied thrust vector
    public Vector3 finalForce = new(0, 0, 0); //final force to be applied this frame

    public float mass = 1.0f;

    public float height;

    public Vector3 impulse = new(0, 0, 0);

    public float timeScalar = 1.0f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        handleMovement();
    }

    private void handleMovement()
    {
        //set the rate of integration, which is (almost) equivalent to
        //explosion by mass for impulse calc. problem is, gravity
        //is no longer a constant. but for gameplay, maybe not an issue?
        var forceDeltaTime = Time.deltaTime * timeScalar;

        var curPos = transform.position; //begin position

        //reset final force to the initial force of gravity
        finalForce.Set(0, GRAVITY_CONSTANT * mass, 0);
        finalForce += thrust;


        acceleration = finalForce / mass;
        velocity += acceleration * forceDeltaTime;
        velocity += impulse;

        //move the object
        transform.position += velocity * forceDeltaTime;

        if (transform.position.y < height)
        {
            transform.position = curPos; //hard reset to the surface
            acceleration *= 0;
            velocity *= 0;
        }


        //reset impulse
        impulse *= 0;
    }

    public void reset()
    {
        velocity *= 0;
        acceleration *= 0;
        impulse *= 0;
        thrust *= 0;
    }
}