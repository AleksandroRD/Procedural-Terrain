using System.Collections.Generic;
using UnityEngine;

public static class HeightMapGenerator
{
    public static HeightMap GenerateHeightMap(int size, List<NoiseLayer> noiseLayers, Vector2 sampleCenter)
    {
        float[,] values = new float[size, size];

        float localMinHeight = float.MaxValue;
        float localMaxHeight = float.MinValue;

        for (int i = 0; i < noiseLayers.Count; i++)
        {
            NoiseLayer noiseLayer = noiseLayers[i];
            float heightMultiplyer = noiseLayers[i].heightMultiplyer;

            float[,] featuredNoise = Noise.GeneratePerlinNoise(size, noiseLayer.NoiseSettings, sampleCenter);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    values[x, y] = (values[x, y] + noiseLayer.NoiseInfluence.Evaluate(featuredNoise[x, y]) * heightMultiplyer) * 0.5f;
                }
            }
        }

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (values[x, y] < localMinHeight)
                {
                    localMinHeight = values[x, y];
                }
                if (values[x, y] > localMaxHeight)
                {
                    localMaxHeight = values[x, y];
                }
            }
        }

        return new HeightMap(values, size, localMinHeight, localMaxHeight);
    }
}

public struct HeightMap
{
    public readonly int Size;
    public readonly float[,] Values;

    public readonly float MinHeight;
    public readonly float MaxHeight;
    public HeightMap(float[,] values, int size, float minHeight, float maxHeight)
    {
        Values = values;
        Size = size;
        MinHeight = minHeight;
        MaxHeight = maxHeight;
    }
}

[System.Serializable]
public struct NoiseLayer
{
    public PerlinNoiseSettings NoiseSettings;
    public AnimationCurve NoiseInfluence;
    public float heightMultiplyer;
}
