using System.Collections.Generic;
using UnityEngine;

public static class HeightMapGenerator
{
    public static HeightMap GenerateHeightMap(int size, List<NoiseSettings> featureNoiseLayers, float minHeight, float maxHeight, Vector2 sampleCenter)
    {
        float[,] values = new float[size,size];

        for (int i = 1; i < featureNoiseLayers.Count; i++)
        {
            NoiseSettings noiseSettings = featureNoiseLayers[i];
            float[,] featuredNoise = Noise.GeneratePerlinNoise(size, noiseSettings, sampleCenter);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    values[x, y] = (values[x, y] + Mathf.LerpUnclamped(minHeight, maxHeight, noiseSettings.noiseInfluence.Evaluate(featuredNoise[x, y]))) * 0.5f;
                }
            }
        }

        return new HeightMap(values);
    }
}

public struct HeightMap
{
    public readonly float[,] values;

    public HeightMap(float[,] values)
    {
        this.values = values;
    }
}
