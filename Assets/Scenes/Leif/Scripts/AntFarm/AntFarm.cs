using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AntFarmCellState
{
    Full,
    Empty,
    Ant,
    QueenAnt
}


public class AntFarm : MonoBehaviour
{
    //TODO if hit edge, backtrack
    //TODO history(10)
    public static float scale = 0.005f;

    [Header("General")] public bool gizmo;

    public Transform cellParent;
    public Vector2Int farmSize = new(116, 70);

    [Header("Update")] public float updateInterval = 1;

    public int generation;

    [Header("Ants")] public Vector2Int[] queenAntPositions = { new(10, 10) };

    public int antMoveFreq = 2;
    public int spawnAntFreq = 10; // higher is less frequent
    [SerializeField] private List<QueenAnt> QueenAnts;
    public Texture2D _texture2D;
    public FarmColors farmColors;
    public MeshRenderer meshRenderer;
    public Material material;

    private AntFarmCell[,] AntFarmCells;


    private float timeAlpha;

    public static Vector3 scaleV3 => Vector3.one * scale;

    private void Start()
    {
        var tex = GenerateFarm();
        tex = ApplyColorToTexture(tex);
        material.mainTexture = tex;
        meshRenderer.sharedMaterial = material;
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
        UpdateFarmColors();
    }

    private void UpdateFarmColors()
    {
        farmColors.ant.UpdateStruct();
        farmColors.queenAnt.UpdateStruct();
        farmColors.empty.UpdateStruct();
        for (var i = 0; i < farmColors.dirt.Length; i++)
            farmColors.dirt[i].UpdateStruct();
    }

    private Texture2D InitializeTexture()
    {
        var h = farmSize.y;
        var w = farmSize.x;
        if (_texture2D == null) _texture2D = new Texture2D(w, h, TextureFormat.ARGB32, false);
        if (_texture2D.width != w || _texture2D.height != h)
            _texture2D.Reinitialize(w, h);
        return _texture2D;
    }

    private FarmColor GetColorForIndex(Vector2Int index)
    {
        return AntFarmCells[index.x, index.y].currentColor;
    }

    public Texture2D ApplyColorToTexture(Texture2D tex)
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

        var asd = tex.GetPixelData<byte>(0);
        Debug.Log(asd.Length);
        Debug.Log(data.Length);

        tex.SetPixelData(data, 0);
        tex.Apply();
        return tex;
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


    private Texture2D GenerateFarm()
    {
        var tex = InitializeTexture();
        // texture is created and initialized
        AntFarmCells = new AntFarmCell[farmSize.x, farmSize.y];
        // cells array created
        QueenAnts = new List<QueenAnt>();
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
                QueenAnts.Add(new QueenAnt(nextCell));

            AntFarmCells[x, y] = nextCell;
        }

        return tex;
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

    [Serializable]
    public struct FarmColors
    {
        public FarmColor ant;
        public FarmColor queenAnt;
        public FarmColor empty;
        public FarmColor[] dirt;
    }

    //
    [Serializable]
    public struct FarmColor
    {
        [Range(0, 255)] public int r;
        [Range(0, 255)] public int g;
        [Range(0, 255)] public int b;
        [Range(0, 255)] public int a;

        [ReadOnly] public Color32 color;

        public FarmColor(int r, int g, int b, int a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
            color = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
        }

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