using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D GenerateNoiseTexture(float[,] noise){
        int size = (int)Mathf.Sqrt(noise.Length);
        Texture2D texture = new Texture2D(size,size);
    
        for(int x = 0;x < size;x++){
            for(int y = 0; y < size;y++){

                texture.SetPixel(x,y,Color.Lerp(Color.black, Color.white, noise[x,y]));
            }
        }
        texture.Apply();
        return texture;

    }
}
