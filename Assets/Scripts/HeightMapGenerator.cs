using UnityEngine;

public static class HeightMapGenerator{
    public static HeightMap GenerateHeightMap(int size, NoiseSettings baseNoiseSettings, NoiseSettings featureNoise,AnimationCurve featureNoiseInfluence, float heightMultiplyer,Vector2 sampleCenter){
        float[,] baseNoise =  Noise.GeneratePerlinNoise(size,baseNoiseSettings,sampleCenter);
        float[,] featuredNoise = Noise.GeneratePerlinNoise(size,featureNoise,sampleCenter);

        float[,] values = new float[size,size];
        for(int x = 0; x < size; x++){
            for(int y = 0; y < size; y++){
                values[x,y] = baseNoise[x,y] * featureNoiseInfluence.Evaluate(featuredNoise[x,y]) * heightMultiplyer;
            }
        }
        return new HeightMap(values);
    }
}

public struct HeightMap{

    public readonly float[,] values;

    public HeightMap (float[,] values)
	{
		this.values = values;
	}
} 
