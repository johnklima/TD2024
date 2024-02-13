using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BoidObstacleAvoid1 : MonoBehaviour
{
    private Transform boid;

    private Boids1 boids;

    private Vector3[] obstacleCheckDirections;

    private void Start()
    {
        var col = GetComponent<SphereCollider>();
        col.radius = 1.5f;
        col.isTrigger = true;
        boid = transform.parent;
        boids = boid.GetComponent<Boids1>();
        obstacleCheckDirections = new[]
        {
            boid.forward,
            -boid.up,
            boid.up,
            boid.right,
            -boid.right
        };
    }

    private void OnTriggerExit(Collider other)
    {
        boids.resetAvoid();
    }

    private void OnTriggerStay(Collider other)
    {
        // Bit shift the index of the layer to get a bit mask
        var layerMask = 1 << 6; //ground
        var didHit = false;


        foreach (var direction in obstacleCheckDirections)
        {
            if (!Physics.Raycast(boid.position, direction, out var hit, 10, layerMask)) continue;
            Debug.DrawRay(boid.position, direction * hit.distance, Color.red);
            boids.accumAvoid(hit.point);
            didHit = true;
        }

        if (!didHit)
            boids.resetAvoid();
    }
}