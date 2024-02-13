using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Boids1 : MonoBehaviour
{
    public BoidsSettings boidsSettings;
    [DoNotSerialize] public Vector3 avoidObst;

    [DoNotSerialize] public Vector3 velocity; //final velocity

    private float avoidCount;

    // Start is called before the first frame update
    private void Start()
    {
        var pos = new Vector3(Random.Range(-5f, 5), Random.Range(-5f, 5f), Random.Range(-5f, 5));
        var look = new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
        var speed = Random.Range(0f, 3f);


        transform.localPosition = pos;
        transform.LookAt(look);
        velocity = (look - pos) * speed;
    }

    // Update is called once per frame
    private void Update()
    {
        var target = boidsSettings.target;
        if (boidsSettings.seekTarget && target != null)
        {
            Debug.Log("Attacking");
            //if not flocking, its going for a target, usually attacking
            var newVelocity = target.position - transform.position;
            var slerpVel = Vector3.Slerp(newVelocity, velocity, Time.deltaTime * boidsSettings.integrationRate);
            velocity = slerpVel.normalized;
            transform.position += velocity * (Time.deltaTime * boidsSettings.speed);
            transform.LookAt(transform.position + velocity);
            if (Vector3.Distance(transform.position, target.position) < 0.3f)
            {
                //Attack successful, do damage, fly away
                Debug.Log("Hit Target");
                boidsSettings.seekTarget = true;
            }
        }
        else
        {
            boidsSettings.constrainPoint = transform.parent.position; //flock follows player
            var newVelocity = new Vector3(0, 0, 0);
            // rule 1 all boids steer towards center of mass - cohesion
            newVelocity += cohesion() * boidsSettings.cohesionFactor;
            // rule 2 all boids steer away from each other - avoidance        
            newVelocity += separation() * boidsSettings.separationFactor;
            // rule 3 all boids match velocity - alignment
            newVelocity += align() * boidsSettings.alignFactor;
            newVelocity += constrain() * boidsSettings.constrainFactor;
            newVelocity += avoid() * boidsSettings.avoidFactor;
            var slerpVel = Vector3.Slerp(velocity, newVelocity, Time.deltaTime * boidsSettings.integrationRate);
            velocity = slerpVel.normalized;
            transform.position += velocity * (Time.deltaTime * boidsSettings.speed);
            transform.LookAt(transform.position + velocity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, .5f);
    }

    private Vector3 avoid()
    {
        if (avoidCount > 0) return (avoidObst / avoidCount).normalized;

        return Vector3.zero;
    }

    private Vector3 constrain()
    {
        var steer = new Vector3(0, 0, 0);

        steer += boidsSettings.constrainPoint - transform.position;

        steer.Normalize();

        return steer;
    }

    private Vector3 cohesion()
    {
        var steer = new Vector3(0, 0, 0);

        var sibs = 0; //count the boids, it might change

        foreach (Transform boid in transform.parent)
            if (boid != transform)
            {
                steer += boid.transform.position;
                sibs++;
            }

        steer /= sibs; //center of mass is the average position of all        

        steer -= transform.position;

        steer.Normalize();


        return steer;
    }

    private Vector3 separation()
    {
        var steer = new Vector3(0, 0, 0);

        var sibs = 0;


        foreach (Transform boid in transform.parent)
            // if boid is not itself
            if (boid != transform)
                // if this boids position is within the collision distance of a neighbouring boid
                if ((transform.position - boid.transform.position).magnitude < boidsSettings.collisionDistance)
                {
                    // our vector becomes this boids pos - neighbouring boids pos
                    steer += transform.position - boid.transform.position;
                    sibs++;
                }

        steer /= sibs;
        steer.Normalize(); //unit, just direction
        return steer;
    }

    private Vector3 align()
    {
        var steer = new Vector3(0, 0, 0);
        var sibs = 0;

        foreach (Transform boid in transform.parent)
            if (boid != transform)
            {
                steer += boid.GetComponent<Boids1>().velocity;
                sibs++;
            }

        steer /= sibs;

        steer.Normalize();

        return steer;
    }

    public void accumAvoid(Vector3 avoid)
    {
        avoidObst += transform.position - avoid;
        avoidCount++;
    }

    public void resetAvoid()
    {
        avoidCount = 0;
        avoidObst *= 0;
    }
}