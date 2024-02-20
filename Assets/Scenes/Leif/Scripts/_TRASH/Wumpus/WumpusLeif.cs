using System;
using System.Collections;
using System.Collections.Generic;
using Scenes.Leif.Scripts;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class WumpusLeif : MonoBehaviour
{
    public Material boxMaterial, pitMaterial, stenchMaterial, breezeMaterial;

    public List<Box> boxes = new();
    public bool asd;
    public INT2 gridSize = new(4, 4);

    public WumpusOperator wumpusOperator = new(new Vector2(1, 1));
    [ReadOnly] public int generation;
    public float generationUpdateFreqInSec = 1;
    private Box[,] grid = new Box[4, 4];

    private void Start()
    {
        StartCoroutine(ProgressGeneration());
    }

    private void OnDrawGizmos()
    {
        wumpusOperator.Draw();
    }

    private void OnValidate()
    {
        if (asd) return;
        if (!asd) asd = true;
        ResetGeneration();
        grid = new Box[gridSize.x, gridSize.y];
        var (_w, _h) = gridSize;
        if (transform.childCount < _h * _w)
            while (transform.childCount < _h * _w)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.parent = transform;
                cube.transform.localScale = Vector3.one * .9f;
            }


        boxes.Clear();
        var index = 0;
        for (var i = 0; i < _w; i++)
        for (var j = 0; j < _h; j++)
        {
            var newBox = new Box(transform.GetChild(index).gameObject, this);
            newBox.position = new Vector3(i, j, 0);
            grid[i, j] = newBox;
            boxes.Add(newBox);
            index++;
        }
    }


    private IEnumerator ProgressGeneration()
    {
        yield return new WaitForSeconds(generationUpdateFreqInSec);
        generation++;
        StartCoroutine(ProgressGeneration());
    }

    private void ResetGeneration()
    {
        StopCoroutine(ProgressGeneration());
        generation = 0;
        wumpusOperator = new WumpusOperator(new Vector2(0, 0));
    }
}

[Serializable]
public class Box
{
    public GameObject gameObject;
    public bool isPit;
    public bool isBreeze;
    public bool isStench;
    public bool isGlitter;

    [SerializeField] private Vector3 _position;
    private MeshRenderer _meshRenderer;
    private WumpusLeif wumpusLeif;

    public Box(GameObject gameObject, WumpusLeif wumpusLeif)
    {
        this.gameObject = gameObject;
        this.wumpusLeif = wumpusLeif;
        var rand = Random.Range(0f, 1f);
        isPit = rand < 0.2f;
        Debug.Log("isPit: " + rand);
        _meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        _meshRenderer.sharedMaterial = this.wumpusLeif.boxMaterial;
        if (isPit) _meshRenderer.sharedMaterial = this.wumpusLeif.pitMaterial;
    }

    public Vector3 position
    {
        get
        {
            gameObject.transform.position = _position;
            return _position;
        }

        set
        {
            gameObject.transform.position = value;
            _position = value;
        }
    }
}

[Serializable]
public class WumpusOperator
{
    public Vector2 position;
    public Color color = Color.black;
    public INT2 pos;

    public WumpusOperator(Vector2 pos)
    {
        position = pos;
    }

    public Box[] GetNeighbours(Box[,] grid)
    {
        List<Box> neighbours = new();
        for (var i = pos.x - 1; i < pos.x + 1; i++)
        for (var j = pos.y - 1; j < pos.y + 1; j++)
        {
            if (i < 0 || i > 4 || j < 0 || j > 4) continue;
            neighbours.Add(grid[i, j]);
        }

        return neighbours.ToArray();
    }

    public void Draw()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere((Vector3)position - Vector3.forward * .5f, .25f);
    }
}