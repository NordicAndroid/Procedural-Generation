using UnityEngine;

public class FractalNoise : Noise
{
    public int layers;
    public Color[] pixels = new Color[256*256];

    //Method that calculates the texture
    public override Texture2D CalculateTexture(){
        for (float y = 0.0f; y < textureHeight; y++){
            for (float x = 0.0f; x < textureWidth; x++) {
                //Calculates the coordinates for where the noise is sampled.
                float xCoord = xOrigin + x / textureWidth * scale;
                float yCoord = yOrigin + y / textureHeight * scale;
                //Stores the output of the noise function in a variable
                //float sample = PerlinNoise.noise(xCoord, yCoord);
                float sample = 1f;
                //Stores the sample in the pixels array.
                pixels[(int)y * textureWidth + (int)x] = new Color(sample, sample, sample);
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}
