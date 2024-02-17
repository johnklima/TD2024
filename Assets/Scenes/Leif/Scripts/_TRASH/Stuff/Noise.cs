using System;
using UnityEngine;
using Random = System.Random;

namespace Scenes.Leif.Scripts
{
    [Serializable]
    public class NoiseData
    {
        [Range(0, 9999)] public int seed;
        [Range(0.0001f, 50f)] public float scale;
        [Range(0, 12)] public int octaves;
        [Range(0, 12)] public float persistence;
        [Range(0, 12)] public float lacunarity;
        public Vector2 offset;
        public NormalizeMode normalizeMode;
    }

    public enum NormalizeMode
    {
        Local,
        Global
    }

    public static class Noise
    {
        public static float[,] GenerateNoiseMap(MapData mapData)
        {
            var noiseData = mapData.noiseData;
            var mapWidth = mapData.mapSize.x;
            var mapHeight = mapData.mapSize.z;
            var seed = noiseData.seed;
            var scale = noiseData.scale;
            var octaves = noiseData.octaves;
            var persistence = noiseData.persistence;
            var lacunarity = noiseData.lacunarity;
            var offset = noiseData.offset;
            var normalizeMode = noiseData.normalizeMode;

            var noiseMap = new float[mapWidth, mapHeight];

            var prng = new Random(seed);
            var octaveOffsets = new Vector2[octaves];

            float maxPossibleHeight = 0;
            float amplitude = 1;
            float frequency = 1;

            for (var i = 0; i < octaves; i++)
            {
                var offsetX = prng.Next(-100000, 100000) + offset.x;
                var offsetY = prng.Next(-100000, 100000) - offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);

                maxPossibleHeight += amplitude;
                amplitude *= persistence;
            }

            if (scale <= 0) scale = 0.0001f;

            var maxLocalNoiseHeight = float.MinValue;
            var minLocalNoiseHeight = float.MaxValue;

            var halfWidth = mapWidth / 2;
            var halfHeight = mapHeight / 2;


            for (var y = 0; y < mapHeight; y++)
            for (var x = 0; x < mapWidth; x++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;


                for (var i = 0; i < octaves; i++)
                {
                    var sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                    var sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;
                    var perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxLocalNoiseHeight) maxLocalNoiseHeight = noiseHeight;
                else if (noiseHeight < minLocalNoiseHeight) minLocalNoiseHeight = noiseHeight;

                noiseMap[x, y] = noiseHeight;
            }

            for (var y = 0; y < mapHeight; y++)
            for (var x = 0; x < mapWidth; x++)

                if (normalizeMode == NormalizeMode.Local)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                }
                else
                {
                    var normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / .9f);
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                }


            return noiseMap;
        }
    }
}