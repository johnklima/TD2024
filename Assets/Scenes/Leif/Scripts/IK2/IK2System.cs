using System;
using UnityEngine;

//system
// creates the threads
// has boids for targets

// thread
// number of segments, siblings, transforms


[Serializable]
public class IK2Settings
{
    public float chainLength = 3;
    public int numberOfChains = 1; // number of segments
    public int segmentsPrChain = 3; //segment.numberOfJoints
    public bool isReaching;
    public bool isDragging;
    [Range(0, 1)] public float wobbleSize = 1;
    [Range(0, 1)] public float sineForce = 1;
}

public class IK2System : MonoBehaviour
{
    public Transform tempTarget; //TODO replace with boids 
    public IK2Settings iK2Settings;
    public IK2Chain[] chains;
    public Transform boids;
    public bool useBoidsAsTarget;

    public GameObject snek;
    private BoidSystem _boidSystem;
    private bool isInitialized;

    private void Start()
    {
        if (useBoidsAsTarget)
        {
            _boidSystem = FindObjectOfType<BoidSystem>();

            if (_boidSystem == null || boids == null)
                throw new Exception("To use boids as target: " +
                                    "'Boids' must be set, and the gameObject must have a <BoidSystem>");
        }
    }

    private void Update()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            Initialize();
        }


        if (chains.Length > 0)
            foreach (var ik2Chain in chains)
                ik2Chain.UpdateChain(this);
    }

    private void OnDrawGizmos()
    {
        if (tempTarget)
            Gizmos.DrawCube(tempTarget.position, Vector3.one * 0.5f);
    }

    private void Initialize()
    {
        chains = new IK2Chain[iK2Settings.numberOfChains];
        if (useBoidsAsTarget) // set number of chains to number of boids
            iK2Settings.numberOfChains = boids.childCount;

        for (var i = 0; i < iK2Settings.numberOfChains; i++)
        {
            GameObject newGo;
            if (snek != null)
            {
                //make new snek
                newGo = Instantiate(snek);
                //set snek as child
                newGo.transform.parent = transform;
                // get snek armature
                newGo = newGo.transform.GetChild(0).gameObject;
            }
            else
            {
                newGo = GenerateNewGo("IKChain_" + i, transform);
            }


            var newChain = newGo.AddComponent<IK2Chain>();
            var target = useBoidsAsTarget ? boids.GetChild(i) : tempTarget;
            newChain.Wake(this, target);
            chains[i] = newChain;
        }
    }

    public static GameObject GenerateNewGo(string name, Transform parent)
    {
        return new GameObject
        {
            name = name,
            transform =
            {
                parent = parent
            }
        };
    }
}