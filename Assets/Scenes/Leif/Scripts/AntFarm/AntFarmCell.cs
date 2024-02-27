using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ant
{
    public AntFarmCell currentCell, prevCell;
    public Vector2Int pos;

    public Ant(AntFarmCell cell)
    {
        currentCell = cell;
        pos = currentCell.index;
    }

    public void UpdateAnt(AntFarm antFarm)
    {
        // should move logic
        if (antFarm.generation % antFarm.antMoveFreq != 0) return;
        Move(antFarm);
    }

    private void Move(AntFarm antFarm)
    {
        currentCell.OnExit();
        var randomDirection = AntFarm.GetRandomDirection();
        var nextPos = pos + randomDirection;
        if (nextPos.x >= antFarm.farmSize.x - 1 ||
            nextPos.y >= antFarm.farmSize.y - 1)
            Move(antFarm);
        prevCell = currentCell;
        pos = nextPos;
        currentCell = antFarm.GetCell(nextPos);
        currentCell.OnEnter();
    }
}

[Serializable]
public class QueenAnt
{
    public List<Ant> ants;
    public AntFarmCell currentCell;
    public Vector2Int pos;


    public QueenAnt(AntFarmCell cell)
    {
        ants = new List<Ant>();
        currentCell = cell;
        pos = currentCell.index;
    }

    public void UpdateQueen(AntFarm antFarm)
    {
        var shouldSpawnAnt = antFarm.generation % antFarm.spawnAntFreq == 0;
        if (shouldSpawnAnt)
        {
            var spawnPos = pos + AntFarm.GetRandomDirection();
            var c = antFarm.GetCell(spawnPos);
            SpawnAnt(c);
        }

        foreach (var ant in ants) ant.UpdateAnt(antFarm);
        currentCell.SetCellState(AntFarmCellState.QueenAnt);
    }

    public void SpawnAnt(AntFarmCell cell)
    {
        ants.Add(new Ant(cell));
    }
}

public class AntFarmCell : MonoBehaviour
{
    public Vector2Int index;
    public AntFarmCellState cellState = AntFarmCellState.Empty;
    public AntFarm antFarm;
    private MeshRenderer _meshRenderer;

    public Vector2 pos
    {
        set => transform.localPosition = value;
        get => transform.localPosition;
    }

    public Vector3 posV3
    {
        set => transform.localPosition = value;
        get => transform.localPosition;
    }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        SetCellState(cellState);
    }

    public void OnEnter()
    {
        SetCellState(AntFarmCellState.Ant);
    }

    public void OnExit()
    {
        SetCellState(AntFarmCellState.Empty);
    }

    public void SetCellState(AntFarmCellState newState)
    {
        var newMaterial = antFarm.GetStateMaterial(newState);
        _meshRenderer.sharedMaterial = newMaterial;
    }
}