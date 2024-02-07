using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    
    [SerializeField] Transform flock;
    public float cohesionFactor = 0.2f;
    public float separationFactor = 6.0f;
    public float allignFactor = 1.0f;
    public float constrainFactor = 2.0f;
    public float avoidFactor = 20.0f;
    public float collisionDistance = 6.0f;
    public float speed = 6.0f;
    public Vector3 constrainPoint;
    
    public Vector3 avoidObst;
    public float integrationRate = 3.0f;
    
    //final velocity
    public Vector3 velocity;
    
    //states
    public bool isFlocking = true;
    public Transform target;
    
   
    float avoidCount;


    // Start is called before the first frame update
    void Start()
    {
        flock = transform.parent;
        

        Vector3 pos = new Vector3(Random.Range(0f, 80), Random.Range(0f, 20f), Random.Range(0f, 80));
        Vector3 look = new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
        float speed = Random.Range(0f, 3f);


        transform.localPosition = pos;
        transform.LookAt(look);
        velocity = (look - pos) * speed;

        

    }

    // Update is called once per frame
    void Update()
    {
        

        if (isFlocking)
        {
            constrainPoint = flock.position;  //flock folows player

            Vector3 newVelocity = new Vector3(0, 0, 0);
            // rule 1 all boids steer towards center of mass - cohesion
            newVelocity += cohesion() * cohesionFactor;
    
            // rule 2 all boids steer away from each other - avoidance        
            newVelocity += separation() * separationFactor;
    
            // rule 3 all boids match velocity - allignment
            newVelocity += align() * allignFactor;
    
            newVelocity += constrain() * constrainFactor;
    
            newVelocity += avoid() * avoidFactor;
           
            Vector3 slerpVelo = Vector3.Slerp(velocity, newVelocity, Time.deltaTime * integrationRate);

            velocity = slerpVelo.normalized;
    
            transform.position += velocity * Time.deltaTime * speed;
            transform.LookAt(transform.position + velocity);

        }
        else if(target)  
        {

            Debug.Log("Attacking");

            //if not flocking, its going for a target, usually attacking
            Vector3 newVelocity = target.position - transform.position;

            Vector3 slerpVelo = Vector3.Slerp(newVelocity, velocity, Time.deltaTime * integrationRate);

            velocity = slerpVelo.normalized;

            transform.position += velocity * Time.deltaTime * speed;
            transform.LookAt(transform.position + velocity);

            if(Vector3.Distance(transform.position, target.position) < 0.3f)
            {
                //Attack successfull, do damage, fly away
                Debug.Log("Hit Target");
                isFlocking = true;
                
            }
        }
    }

    Vector3 avoid()
    {

        if (avoidCount > 0)
        {
            return (avoidObst / avoidCount).normalized ;
        }

        return Vector3.zero;
    }
    
    Vector3 constrain()
    {
        Vector3 steer = new Vector3(0, 0, 0);

        steer += (constrainPoint - transform.position);

        steer.Normalize();

        return steer;
    }

    Vector3 cohesion()
    {
        Vector3 steer = new Vector3(0, 0, 0);

        int sibs = 0;           //count the boids, it might change

        foreach (Transform boid in flock)
        {
            if (boid != transform)
            {
                steer += boid.transform.position;
                sibs++;
            }

        }

        steer /= sibs; //center of mass is the average position of all        

        steer -= transform.position;

        steer.Normalize();


        return steer;
    }

    Vector3 separation()
    {
        Vector3 steer = new Vector3(0, 0, 0);

        int sibs = 0;


        foreach (Transform boid in flock)
        {
            // if boid is not itself
            if (boid != transform)
            {
                // if this boids position is within the collision distance of a neighbouring boid
                if ((transform.position - boid.transform.position).magnitude < collisionDistance)
                {
                    // our vector becomes this boids pos - neighbouring boids pos
                    steer += (transform.position - boid.transform.position);
                    sibs++;
                }

            }

        }
        steer /= sibs;
        steer.Normalize();        //unit, just direction
        return steer;

    }

    Vector3 align()
    {
        Vector3 steer = new Vector3(0, 0, 0);
        int sibs = 0;

        foreach (Transform boid in flock)
        {
            if (boid != transform)
            {
                steer += boid.GetComponent<Boids>().velocity;
                sibs++;
            }

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
