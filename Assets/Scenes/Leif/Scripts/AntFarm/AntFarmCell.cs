using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AntFarmCell
{
    public AntFarm antFarm;
    public AntFarmCellState cellState = AntFarmCellState.Empty;

    public AntFarm.FarmColor currentColor;

    // was cube, is pixel
    public Vector2Int index;

    public AntFarmCell(AntFarm antFarm, AntFarmCellState cellState, Vector2Int index)
    {
        this.antFarm = antFarm;
        this.cellState = cellState;
        this.index = index;
        SetCellState(cellState);
    }

    private void Awake()
    {
        // _meshRenderer = GetComponent<MeshRenderer>();
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

    public void SetCellColor(AntFarmCellState newState)
    {
        currentColor = newState switch
        {
            AntFarmCellState.Ant => antFarm.farmColors.ant,
            AntFarmCellState.Full => GetRandomDirtColor32(),
            AntFarmCellState.Empty => antFarm.farmColors.empty,
            AntFarmCellState.QueenAnt => antFarm.farmColors.queenAnt,
            _ => throw new Exception("")
        };
    }


    private AntFarm.FarmColor GetRandomDirtColor32()
    {
        var dirt = antFarm.farmColors.dirt;
        var rand = Random.Range(0, dirt.Length);
        return dirt[rand];
    }

    public void SetCellState(AntFarmCellState newState)
    {
        SetCellColor(newState);
        // var newMaterial = antFarm.GetStateMaterial(newState);
        // _meshRenderer.sharedMaterial = newMaterial;
    }
}