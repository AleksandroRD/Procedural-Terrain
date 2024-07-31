using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DetailGenerator
{
    public static void CreateDetails(WorldChunk worldChunk, List<DetailSettings> detailSettings)
    {
        HeightMap heightMap = worldChunk.HeightMap;
        Transform chunkTransform = worldChunk.gameObject.transform;

        int halfSize = heightMap.Size / 2;
        for (int i = 0; i < detailSettings.Count; i++)
        {
            System.Random rand = new System.Random(detailSettings[i].Seed + i);

            if (worldChunk.HeightMap.MaxHeight < detailSettings[i].MinHeight || worldChunk.HeightMap.MinHeight > detailSettings[i].MaxHeight) { continue; }

            for (int k = 0; k < detailSettings[i].AmountPerChunk;)
            {
                int x = rand.Next(0, heightMap.Size);
                int y = rand.Next(0, heightMap.Size);

                if (worldChunk.HeightMap.Values[x, y] < detailSettings[i].MinHeight || worldChunk.HeightMap.Values[x, y] > detailSettings[i].MaxHeight) { continue; }

                GameObject.Instantiate(detailSettings[i].Detail,
                    chunkTransform.TransformPoint(new Vector3(x - halfSize, worldChunk.HeightMap.Values[x, y], -y + halfSize)),
                    new Quaternion(),
                    chunkTransform);

                k++;

            }
        }
    }
}

[System.Serializable]
public class DetailSettings
{
    public GameObject Detail;
    public int Seed;
    public int AmountPerChunk;
    public float MinHeight;
    public float MaxHeight;
    public float MinAngle;
    public float MaxAngle;
}
