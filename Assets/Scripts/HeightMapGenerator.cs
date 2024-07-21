using System.Collections.Generic;
using UnityEngine;

public static class HeightMapGenerator
{
    public static HeightMap GenerateHeightMap(int size, List<NoiseLayer> noiseLayers, float minHeight, float maxHeight, Vector2 sampleCenter)
    {
        float[,] values = new float[size,size];
        
        for (int i = 0; i < noiseLayers.Count; i++)
        {
            PerlinNoiseSettings noiseSettings = noiseLayers[i].NoiseSettings;
            float[,] featuredNoise = Noise.GeneratePerlinNoise(size, noiseSettings, sampleCenter);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    values[x, y] = (values[x, y] + Mathf.LerpUnclamped(minHeight, maxHeight, noiseLayers[i].NoiseInfluence.Evaluate(featuredNoise[x, y]))) * 0.5f;
                }
            }       
        }

        return new HeightMap(values, size);
    }
}

public struct HeightMap
{
    public readonly int Size;
    public readonly float[,] Values;
    public HeightMap(float[,] values, int size)
    {
        Values = values;
        Size = size;
    }
}

[System.Serializable]
public struct NoiseLayer
{
    public PerlinNoiseSettings NoiseSettings;
    public AnimationCurve NoiseInfluence;
    public float heightMultiplyer;
}
