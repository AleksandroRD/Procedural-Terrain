using UnityEngine;

public static class Noise
{
	public static float[,] GeneratePerlinNoise(int size, NoiseSettings settings, Vector2 sampleCenter)
	{
		float[,] noiseMap = new float[size, size];
		Vector2[] octaveOffsets = new Vector2[settings.octaves];

		System.Random prng = new System.Random(settings.seed);

		float maxPossibleHeight = 0;

		float amplitude = 1;
		float frequency = 1;

		for (int i = 0; i < settings.octaves; i++)
		{
			float offsetX = sampleCenter.x + prng.Next(-100000, 100000) - size * 0.5f;
			float offsetY = -sampleCenter.y + prng.Next(-100000, 100000) - size * 0.5f;

			maxPossibleHeight += amplitude;
			amplitude *= settings.persistance;

			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{

				frequency = 1;
				amplitude = 1;
				float noiseValue = 0;

				for (int i = 0; i < settings.octaves; i++)
				{
					float sampleX = (x + octaveOffsets[i].x) / settings.scale * frequency;
					float sampleY = (y + octaveOffsets[i].y) / settings.scale * frequency;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
					noiseValue += perlinValue * amplitude;

					amplitude *= settings.persistance;
					frequency *= settings.lacunarity;

				}
				noiseMap[x, y] = noiseValue;

				float normalizedHeight = noiseMap[x, y] / (maxPossibleHeight / 0.9f);
				noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
			}
		}

		return noiseMap;
	}

	public static float[,] GenerateVoronoiNoise(int size, int regionAmount, int seed)
	{
		Random.InitState(seed);

		float[,] heightmap = new float[size, size];
		Vector2Int[] centroids = new Vector2Int[regionAmount];

		for (int i = 0; i < regionAmount; i++)
		{
			centroids[i] = new Vector2Int(Random.Range(0, size), Random.Range(0, size));
		}

		float[,] distances = new float[size, size];

		float maxDistance = float.MinValue;

		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				distances[x, y] = Vector2.Distance(new Vector2Int(x, y), centroids[GetClosestCentroidIndex(new Vector2Int(x, y), centroids)]);

				if (distances[x, y] > maxDistance)
				{
					maxDistance = distances[x, y];
				}
			}
		}

		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				float noiseValue = distances[x, y] / maxDistance;
				heightmap[x, y] = Mathf.InverseLerp(0, 1, noiseValue);
			}
		}

		return heightmap;
	}

	public static Texture2D GenerateVoronoiRegions(int regionAmount, int size, Color[] regionColors, int seed)
	{
		Random.InitState(seed);

		Vector2Int[] centroids = new Vector2Int[regionAmount];
		Color[] regions = new Color[regionAmount];

		//generate random center points for regiones
		for (int i = 0; i < regionAmount; i++)
		{
			centroids[i] = new Vector2Int(Random.Range(0, size), Random.Range(0, size));
			regions[i] = regionColors[Random.Range(0, regionColors.Length)];
		}

		Color[] pixelColors = new Color[size * size];

		//set every region random color
		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				int index = x * size + y;
				pixelColors[index] = regions[GetClosestCentroidIndex(new Vector2Int(x, y), centroids)];
			}
		}
		return GetImageFromColorArray(pixelColors, size);
	}

	private static int GetClosestCentroidIndex(Vector2Int pixelPosition, Vector2Int[] centroids)
	{
		float smallestDst = float.MaxValue;
		int index = 0;
		for (int i = 0; i < centroids.Length; i++)
		{
			if (Vector2.Distance(pixelPosition, centroids[i]) < smallestDst)
			{
				smallestDst = Vector2.Distance(pixelPosition, centroids[i]);
				index = i;
			}
		}
		return index;
	}

	private static Texture2D GetImageFromColorArray(Color[] pixelColors, int size)
	{
		Texture2D tex = new(size, size) { filterMode = FilterMode.Point };

		tex.SetPixels(pixelColors);
		tex.Apply();

		return tex;
	}

}

[System.Serializable]
public class NoiseSettings
{
	public int seed;
	public float scale = 50;
	public int octaves = 6;
	[Range(0, 1)] public float persistance = .6f;
	public float lacunarity = 2;
	public Vector2 offset;
	public AnimationCurve noiseInfluence;
}
