using System;
using System.Collections.Generic;
using UnityEngine;

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