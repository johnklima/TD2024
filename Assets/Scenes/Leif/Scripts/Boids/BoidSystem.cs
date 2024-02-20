using System;
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
            boids[i] = CreateBoid(prefab);
    }

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        if (boidsSettings.constrainPoint != null && boidsSettings.useConstrainPoint)
            Gizmos.DrawWireCube(boidsSettings.constrainPoint.position, Vector3.one * 3);
        else
            Gizmos.DrawWireCube(transform.position, Vector3.one * 3);
        Gizmos.color = Color.red;
        if (boidsSettings.target != null)
            Gizmos.DrawWireCube(boidsSettings.target.position, Vector3.one * 3);
    }

    private GameObject CreateBoid(GameObject prefab = null)
    {
        //TODO instantiate prefab
        var parent = transform;
        GameObject boidGo;
        if (prefab) boidGo = Instantiate(prefab);
        else boidGo = new GameObject();

        var prefabIndicator = prefab == null ? "" : $"_prefab({prefab.name})";
        boidGo.name = $"Boid_{parent.childCount}{prefabIndicator}";
        boidGo.transform.localPosition = Vector3.zero;
        boidGo.transform.parent = parent;


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

[Serializable]
public class BoidsSettings
{
    public float cohesionFactor = 0.2f;
    public float separationFactor = 6.0f;
    public float alignFactor = 1.0f;
    public float constrainFactor = 2.0f;
    public float avoidFactor = 20.0f;
    public float collisionDistance = 6.0f;
    public float speed = 6.0f;
    public float integrationRate = 3.0f;
    public LayerMask obstacleLayerMask;

    //states
    public bool seekTarget;
    public Transform target;
    public bool useConstrainPoint;
    public Transform constrainPoint;
}