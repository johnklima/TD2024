using System;
using UnityEngine;

public class TestAntFarm : MonoBehaviour
{
    public int w = 116, h = 70;

    public MeshRenderer meshRenderer;
    public Material material;
    public Texture2D _texture2D;

    // public TestColor testColor = new(255, 0, 0, 0);

    // private void OnValidate()
    // {
    //     InitializeTexture();
    //     testColor.UpdateStruct();
    // }

    private void InitializeTexture()
    {
        if (_texture2D == null) _texture2D = new Texture2D(w, h, TextureFormat.ARGB32, false);
        if (_texture2D.width != w || _texture2D.height != h)
            _texture2D.Reinitialize(w, h);
    }

    public void GenerateColor()
    {
        if (meshRenderer == null) throw new Exception("make sure meshRenderer is set");
        if (material == null) throw new Exception("make sure material is set");
        var width = _texture2D.width;
        var height = _texture2D.height;

        var data = new byte[width * height * 4];


        // var currentPix = 0;
        // for (var y = 0; y < height; y++)
        // for (var x = 0; x < width; x++)
        // {
        //     data[currentPix] = (byte)testColor.r;
        //     data[currentPix + 1] = (byte)testColor.g;
        //     data[currentPix + 2] = (byte)testColor.b;
        //     data[currentPix + 3] = (byte)testColor.a;
        //     currentPix += 4;
        // }

        var asd = _texture2D.GetPixelData<byte>(0);
        Debug.Log(asd.Length);
        Debug.Log(data.Length);

        _texture2D.SetPixelData(data, 0);
        _texture2D.Apply();
        material.mainTexture = _texture2D;
        meshRenderer.sharedMaterial = material;
    }
    //
    // [Serializable]
    // public struct TestColor
    // {
    //     [Range(0, 255)] public int r;
    //     [Range(0, 255)] public int g;
    //     [Range(0, 255)] public int b;
    //     [Range(0, 255)] public int a;
    //
    //     [ReadOnly] public Color32 color;
    //
    //     public TestColor(int r, int g, int b, int a)
    //     {
    //         this.r = r;
    //         this.g = g;
    //         this.b = b;
    //         this.a = a;
    //         color = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
    //     }
    //
    //     public void UpdateStruct()
    //     {
    //         color = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
    //     }
    // }
}