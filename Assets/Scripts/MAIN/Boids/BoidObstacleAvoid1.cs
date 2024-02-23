using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BoidObstacleAvoid1 : MonoBehaviour
{
    private LayerMask _layerMask;
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

        _layerMask = boids.boidsSettings.obstacleLayerMask;
    }

    private void OnTriggerExit(Collider other)
    {
        boids.resetAvoid();
    }

    private void OnTriggerStay(Collider other)
    {
        // Bit shift the index of the layer to get a bit mask
        var didHit = false;
        obstacleCheckDirections = new[]
        {
            boid.forward,
            -boid.up,
            boid.up,
            boid.right,
            -boid.right
        };
        foreach (var direction in obstacleCheckDirections)
        {
            Debug.DrawRay(boid.position, direction * 10, Color.red);
            if (!Physics.Raycast(boid.position, direction, out var hit, 10, _layerMask)) continue;
            boids.accumAvoid(hit.point);
            didHit = true;
        }

        if (!didHit)
            boids.resetAvoid();
    }
}