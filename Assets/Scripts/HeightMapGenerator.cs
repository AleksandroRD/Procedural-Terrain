using System.Collections.Generic;
using UnityEngine;

public static class HeightMapGenerator
{
    public static HeightMap GenerateHeightMap(int size, NoiseSettings baseNoiseSettings, List<NoiseSettings> featureNoiseLayers, float minHeight, float maxHeight, Vector2 sampleCenter){
        float[,] values = Noise.GeneratePerlinNoise(size, baseNoiseSettings, sampleCenter);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                values[x, y] = Mathf.Lerp(minHeight, maxHeight, baseNoiseSettings.noiseInfluence.Evaluate(values[x, y]));
            }
        }

        for (int i = 0; i < featureNoiseLayers.Count; i++)
        {
            float[,] featuredNoise = Noise.GeneratePerlinNoise(size, featureNoiseLayers[i], sampleCenter);

            for (int x = 0; x < size; x++){
                for(int y = 0; y < size; y++){
                    values[x,y] = (values[x, y] + Mathf.Lerp(minHeight, maxHeight, featureNoiseLayers[i].noiseInfluence.Evaluate(featuredNoise[x, y])))  * 0.5f;
                }
            }
        }

        return new HeightMap(values);
    }
}

public struct HeightMap
{
    public readonly float[,] values;

    public HeightMap (float[,] values)
	{
		this.values = values;
	}
} 
