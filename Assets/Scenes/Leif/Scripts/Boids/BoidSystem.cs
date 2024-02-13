using UnityEngine;

public class BoidSystem : MonoBehaviour
{
    public int numberOfBoids;
    public GameObject prefab;
    public BoidsSettings boidsSettings;

    public GameObject[] boids;

    private void Start()
    {
        boids = new GameObject[numberOfBoids];
        for (var i = 0; i < numberOfBoids; i++)
            boids[i] = CreateBoid();
    }

    private void Update()
    {
    }

    private GameObject CreateBoid()
    {
        var parent = transform;
        var boidGo = new GameObject($"Boid_{parent.childCount}")
        {
            hideFlags = HideFlags.None,
            transform =
            {
                localPosition = Vector3.zero,
                parent = parent
            }
        };
        var boid = boidGo.AddComponent<Boids1>();
        boid.boidsSettings = boidsSettings;

        new GameObject(
            $"ObstacleAvoid_{parent.childCount}",
            typeof(BoidObstacleAvoid1)
        )
        {
            transform =
            {
                parent = boidGo.transform
            }
        };
        return boidGo;
    }
}