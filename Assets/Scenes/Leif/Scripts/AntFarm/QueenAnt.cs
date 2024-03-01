using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class QueenAnt
{
    public List<Ant> ants;
    public Vector2Int pos;
    public AntFarmCell currentCell;


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
            var spawnPos = pos;
            if (ants.Count > 0)
            {
                var ant = ants[Random.Range(0, ants.Count)];
                spawnPos = ant.pos + ant.GetRandomDirection();
            }

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