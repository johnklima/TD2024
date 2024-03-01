using System;
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
            nextPos.Clamp(Vector2Int.zero, antFarm.farmSize);
        Move(antFarm);
        prevCell = currentCell;
        pos = nextPos;
        currentCell = antFarm.GetCell(nextPos);
        currentCell.OnEnter();
    }
}