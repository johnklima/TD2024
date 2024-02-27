using UnityEngine;

public class FishBoidSystem : MonoBehaviour
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

    private void OnDrawGizmosSelected()
    {
        if (boidsSettings.constrainPoint != null && boidsSettings.useConstrainPoint)
            Gizmos.DrawWireCube(boidsSettings.constrainPoint.position, Vector3.one * 3);
        else
            Gizmos.DrawWireCube(transform.position, Vector3.one * 3);
        Gizmos.color = Color.red;
        if (boidsSettings.target != null)
            Gizmos.DrawWireCube(boidsSettings.target.position, Vector3.one * 3);
    }

    public void FishAttack()
    {
        if (boidsSettings.target != null) boidsSettings.seekTarget = true;
        Debug.Log("FishAttack!");
    }

    public void TestFishAttack()
    {
        Debug.Log("TestFishAttack Success!");
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


        var boid = boidGo.AddComponent<FishBoids1>();
        boid.boidsSettings = boidsSettings;

        new GameObject(
            $"ObstacleAvoid_{parent.childCount}",
            typeof(FishBoidObstacleAvoid1)
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