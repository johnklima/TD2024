using System;
using System.Collections.Generic;
using Scenes.Leif.Scripts;
using UnityEngine;

[ExecuteAlways]
public class Voxel : MonoBehaviour
{
    public Action<Int3> voxelStateChange, voxelStateDestroy;
    public Int3 coordinates;
    public bool isSolid;
    public bool isVisible;
    public float noise;
    public List<Voxel> neighbours = new();

    private void OnEnable()
    {
        voxelStateChange?.Invoke(coordinates);
    }

    private void OnDisable()
    {
        RemoveFromNeighbours();
        voxelStateDestroy?.Invoke(coordinates);
    }

    private void OnDestroy()
    {
        RemoveFromNeighbours();
        voxelStateDestroy?.Invoke(coordinates);
    }

    private void RemoveFromNeighbours()
    {
        if (neighbours == null) return;
        foreach (var neighbour in neighbours) neighbour.neighbours.Remove(this);
    }

    public void RegisterEvents(Action<Int3> voxelStateChangeAction, Action<Int3> voxelStateDestroyAction)
    {
        voxelStateChange = voxelStateChangeAction;
        voxelStateDestroy = voxelStateDestroyAction;
    }
}