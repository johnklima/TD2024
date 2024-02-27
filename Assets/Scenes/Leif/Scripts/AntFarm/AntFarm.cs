using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AntFarmCellState
{
    Full,
    Empty,
    Ant,
    QueenAnt
}

[Serializable]
public struct AntFarmMaterials
{
    public Material fullMaterial, emptyMaterial, antMaterial, queenAntMaterial;
}

public class AntFarm : MonoBehaviour
{
    //TODO if hit edge, backtrack
    //TODO history(10)
    public static float scale = 0.005f;

    [Header("General")] public AntFarmMaterials antFarmMaterials;
    public bool gizmo;
    public GameObject antFarmCellPrefab;
    public Transform cellParent;
    public Vector2Int farmSize = new(115, 70);

    [Header("Update")] public float updateInterval = 1;

    public int generation;

    [Header("Ants")] public Vector2Int[] queenAntPositions = { new(10, 10) };

    public int antMoveFreq = 2;
    public int spawnAntFreq = 10; // higher is less frequent
    [SerializeField] private List<QueenAnt> QueenAnts;

    private AntFarmCell[,] AntFarmCells;

    private GameObject[,] GameObjects;

    private float timeAlpha;

    public static Vector3 scaleV3 => Vector3.one * scale;

    private void Start()
    {
        GenerateGameObjects();
    }

    private void Update()
    {
        timeAlpha += Time.deltaTime;
        if (timeAlpha < updateInterval) return;
        timeAlpha = 0;
        UpdateCellLoop();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying || !gizmo) return;
        for (var y = 0; y < farmSize.y; y++)
        for (var x = 0; x < farmSize.x; x++)
        {
            var pos = cellParent.position + new Vector3(x * scale + scale / 2f, y * scale + scale / 2f, 0);
            foreach (var q in queenAntPositions)
                if (q.x == x && q.y == y)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(pos, scaleV3);
                    break;
                }

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(pos, scaleV3);
        }

        Gizmos.color = Color.red;
        var pos_ = new Vector3(scale / 2f, scale / 2f, 0);
        Gizmos.DrawWireCube(cellParent.position + pos_, scaleV3);
    }

    private void OnValidate()
    {
        foreach (var q in queenAntPositions)
            q.Clamp(Vector2Int.zero, farmSize);
    }

    public static Vector2Int GetRandomDirection()
    {
        var rand = Random.Range(0, 1f);
        return rand switch
        {
            <= .25f => Vector2Int.up,
            (> .25f and <= .5f) => Vector2Int.down,
            (> .5f and <= .75f) => Vector2Int.left,
            (> .75f and <= 1f) => Vector2Int.right,
            _ => throw new ArgumentOutOfRangeException(nameof(rand), rand, null)
        };
    }

    public AntFarmCell GetCell(Vector2Int i)
    {
        return AntFarmCells[i.x, i.y];
    }

    public Material GetStateMaterial(AntFarmCellState state)
    {
        return state switch
        {
            AntFarmCellState.Full => antFarmMaterials.fullMaterial,
            AntFarmCellState.Empty => antFarmMaterials.emptyMaterial,
            AntFarmCellState.Ant => antFarmMaterials.antMaterial,
            AntFarmCellState.QueenAnt => antFarmMaterials.queenAntMaterial,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }


    private void GenerateGameObjects()
    {
        GameObjects = new GameObject[farmSize.x, farmSize.y];
        AntFarmCells = new AntFarmCell[farmSize.x, farmSize.y];
        QueenAnts = new List<QueenAnt>();
        for (var y = 0; y < farmSize.y; y++)
        for (var x = 0; x < farmSize.x; x++)
        {
            var pos = new Vector3(x * scale + scale / 2f, y * scale + scale / 2f, 0);
            var prefab = Instantiate(antFarmCellPrefab, cellParent);
            prefab.SetActive(true);
            prefab.name += x + "_" + y + "_" + cellParent.childCount;
            var cell = prefab.GetComponent<AntFarmCell>();
            cell.posV3 = pos;
            cell.index = new Vector2Int(x, y);
            var cellTr = cell.transform;
            cellTr.localScale = scaleV3;
            cellTr.localRotation = Quaternion.identity;
            var startState = GetStartState(cell.index);
            if (startState == AntFarmCellState.QueenAnt)
                QueenAnts.Add(new QueenAnt(cell));
            cell.SetCellState(startState);
            GameObjects[x, y] = prefab;
            AntFarmCells[x, y] = cell;
        }
    }

    private AntFarmCellState GetStartState(Vector2Int index)
    {
        if (queenAntPositions.Contains(index))
            return AntFarmCellState.QueenAnt;
        return AntFarmCellState.Full;
    }

    private void UpdateCellLoop()
    {
        generation++;
        if (QueenAnts.Count > 0)
            for (var i = 0; i < QueenAnts.Count; i++)
            {
                var q = QueenAnts[i];
                q.UpdateQueen(this);
            }
    }
}