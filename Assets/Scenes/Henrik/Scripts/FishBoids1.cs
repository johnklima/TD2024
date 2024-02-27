using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FishBoids1 : MonoBehaviour
{
    public BoidsSettings boidsSettings;
    [DoNotSerialize] public Vector3 avoidObst;

    [DoNotSerialize] public Vector3 velocity; //final velocity

    private Vector3 _constrainPoint;

    private float avoidCount;

    private FishBoids1[] fishBoids;

    // Start is called before the first frame update
    private void Start()
    {
        if (boidsSettings.useConstrainPoint && boidsSettings.constrainPoint == null)
            boidsSettings.constrainPoint = transform.parent;

        var pos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        var look = new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
        var speed = Random.Range(0f, 3f);


        transform.localPosition = pos;
        transform.LookAt(look);
        velocity = (look - pos) * speed;
        fishBoids = transform.parent.GetComponentsInChildren<FishBoids1>();
    }

    // Update is called once per frame
    private void Update()
    {
        var pos = transform.position;
        if (boidsSettings.target != null && boidsSettings.seekTarget)
        {
            var target = boidsSettings.target;
            Debug.Log("Attacking");
            //if not flocking, its going for a target, usually attacking
            var newVelocity = target.position - pos;
            var slerpVel = Vector3.Slerp(newVelocity, velocity, Time.deltaTime * boidsSettings.integrationRate);
            velocity = slerpVel.normalized;
            transform.position += velocity * (Time.deltaTime * boidsSettings.speed);
            transform.LookAt(pos + velocity);
            if (Vector3.Distance(pos, target.position) < 0.3f)
            {
                //Attack successful, do damage, fly away
                Debug.Log("Hit Target");
                boidsSettings.seekTarget = false;
                boidsSettings.onHitTarget.Invoke();
            }
        }
        else
        {
            _constrainPoint = boidsSettings.useConstrainPoint && boidsSettings.constrainPoint != null
                ? boidsSettings.constrainPoint.position
                : transform.parent.position; //flock follows player
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
            transform.LookAt(pos + velocity);
        }

        var _pos = transform.position;
        var distFromParentY = boidsSettings.yConstrainDistance;
        var parentY = transform.parent.position.y;
        _pos.y = Mathf.Clamp(_pos.y, parentY - distFromParentY, parentY + distFromParentY);
        transform.position = _pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, .5f);
        if (boidsSettings.target != null && boidsSettings.seekTarget)
        {
            var target = boidsSettings.target;
        }
    }

    private Vector3 avoid()
    {
        if (avoidCount > 0) return (avoidObst / avoidCount).normalized;

        return Vector3.zero;
    }

    private Vector3 constrain()
    {
        var steer = new Vector3(0, 0, 0);

        steer += _constrainPoint - transform.position;

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


        foreach (var boid in fishBoids)
            if (boid != this)
            {
                steer += boid.velocity;
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