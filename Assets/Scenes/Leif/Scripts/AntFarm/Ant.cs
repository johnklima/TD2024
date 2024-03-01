using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Ant
{
    private static Vector2Int[] _directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public Vector2Int pos;

    private int curAttempt, maxAttempt = 10;

    public AntFarmCell currentCell, prevCell;

    private Vector2Int prevDir;

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

    public Vector2Int GetRandomDirection()
    {
        curAttempt++;
        var rand = Random.Range(0, _directions.Length);
        if (_directions[rand] == prevDir)
            if (curAttempt < maxAttempt)
                return GetRandomDirection();

        curAttempt = 0;
        return _directions[rand];
    }

    private void Move(AntFarm antFarm, bool re = false)
    {
        if (!re)
            currentCell.OnExit();

        var randomDirection = GetRandomDirection();
        var nextPos = pos + randomDirection;
        // check if outside of texture 

        nextPos.Clamp(Vector2Int.zero, antFarm.farmSize - Vector2Int.one);


        prevDir = randomDirection;
        prevCell = currentCell;
        pos = nextPos;
        currentCell = antFarm.GetCell(nextPos);
        currentCell.OnEnter();
    }
}