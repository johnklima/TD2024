using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public enum AntFarmCellState
{
    Full,
    Empty,
    Ant,
    QueenAnt
}


public class AntFarm : MonoBehaviour
{
    [Header("General")] public Vector2Int farmSize = new(116, 70);

    [Header("Update")] public float updateInterval = 1;
    [ReadOnly] public int generation;

    [Header("Ants")] public Vector2Int[] queenAntPositions = { new(10, 10) };

    public int antMoveFreq = 2;
    public int spawnAntFreq = 10; // higher is less frequent

    [Header("Colors")] public FarmColors farmColors;

    [Header("Texture settings")] public MeshRenderer meshRenderer;

    public Material material;

    private AntFarmCell[,] _antFarmCells;
    private float _timeAlpha;
    private List<QueenAnt> queenAnts;
    private Texture2D texture2D;


    private void Start()
    {
        GenerateFarm();
    }

    private void Update()
    {
        _timeAlpha += Time.deltaTime;
        if (_timeAlpha < updateInterval) return;
        _timeAlpha = 0;
        UpdateCellLoop();
    }


    private void OnValidate()
    {
        for (var i = 0; i < queenAntPositions.Length; i++)
            queenAntPositions[i].Clamp(Vector2Int.zero, farmSize - Vector2Int.one);
        UpdateFarmColors();
        InitializeTexture();
    }

    private void UpdateCellLoop()
    {
        generation++;
        if (queenAnts.Count <= 0) return;
        for (var i = 0; i < queenAnts.Count; i++)
            queenAnts[i].UpdateQueen(this);
        UpdateTexture();
    }

    private void UpdateTexture()
    {
        texture2D = ApplyColorToTexture(texture2D);
        material.mainTexture = texture2D;
        meshRenderer.sharedMaterial = material;
    }

    private Texture2D ApplyColorToTexture(Texture2D tex)
    {
        if (meshRenderer == null) throw new Exception("make sure meshRenderer is set");
        if (material == null) throw new Exception("make sure material is set");
        var width = tex.width;
        var height = tex.height;

        var data = new byte[width * height * 4];

        var currentPix = 0;
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var index = new Vector2Int(x, y);
            var color = GetColorForIndex(index).ToByteArray();
            data[currentPix + 0] = color[0];
            data[currentPix + 1] = color[1];
            data[currentPix + 2] = color[2];
            data[currentPix + 3] = color[3];
            currentPix += 4;
        }

        tex.SetPixelData(data, 0);
        tex.Apply();
        return tex;

        FarmColor GetColorForIndex(Vector2Int index)
        {
            return _antFarmCells[index.x, index.y].currentColor;
        }
    }


    private void GenerateFarm()
    {
        InitializeTexture();
        // texture is created and initialized
        _antFarmCells = new AntFarmCell[farmSize.x, farmSize.y];
        // cells array created
        queenAnts = new List<QueenAnt>();
        // queen list created
        for (var y = 0; y < farmSize.y; y++)
        for (var x = 0; x < farmSize.x; x++)
        {
            var index = new Vector2Int(x, y);
            // get start state
            var startState = GetStartState(index);
            // instantiate a cell (base color)
            var nextCell = new AntFarmCell(this, startState, index);
            if (startState == AntFarmCellState.QueenAnt)
                queenAnts.Add(new QueenAnt(nextCell));

            _antFarmCells[x, y] = nextCell;
        }
    }

    private void InitializeTexture()
    {
        var h = farmSize.y;
        var w = farmSize.x;
        if (texture2D == null)
            texture2D = new Texture2D(w, h, TextureFormat.RGBA32, false)
            {
                name = "proc gen tex"
            };
        if (texture2D.width != w || texture2D.height != h)
            texture2D.Reinitialize(w, h);
    }

    private AntFarmCellState GetStartState(Vector2Int index)
    {
        if (queenAntPositions.Contains(index))
            return AntFarmCellState.QueenAnt;
        return AntFarmCellState.Full;
    }


    public AntFarmCell GetCell(Vector2Int i)
    {
        var x = Math.Clamp(i.x, 0, _antFarmCells.GetLength(0) - 1);
        var y = Math.Clamp(i.y, 0, _antFarmCells.GetLength(1) - 1);
        return _antFarmCells[x, y];
    }

    private void UpdateFarmColors()
    {
        farmColors.ant.UpdateStruct();
        farmColors.queenAnt.UpdateStruct();
        farmColors.empty.UpdateStruct();
        for (var i = 0; i < farmColors.dirt.Length; i++)
            farmColors.dirt[i].UpdateStruct();
    }

    [Serializable]
    public struct FarmColors
    {
        public FarmColor ant;
        public FarmColor queenAnt;
        public FarmColor empty;
        public FarmColor[] dirt;
    }

    [Serializable]
    public struct FarmColor
    {
        [Range(0, 255)] public int r;
        [Range(0, 255)] public int g;
        [Range(0, 255)] public int b;
        [Range(0, 255)] public int a;

        [ReadOnly] public Color32 color;


        public Color32 ToColor32()
        {
            return new Color32((byte)r, (byte)g, (byte)b, (byte)a);
        }

        public byte[] ToByteArray()
        {
            return new[] { (byte)r, (byte)g, (byte)b, (byte)a };
        }

        public void UpdateStruct()
        {
            color = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
        }
    }
}