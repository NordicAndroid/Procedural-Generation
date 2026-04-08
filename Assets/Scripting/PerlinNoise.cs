using UnityEngine;

public class PerlinNoise : Noise
{
    private Color[] pixels;
    public override Texture2D CalculateTexture(){
        for (float y = 0.0f; y < textureHeight; y++){
            for (float x = 0.0f; x < textureWidth; x++) {
                float xCoord = xOrigin + x / textureWidth * scale;
                float yCoord = yOrigin + y / textureHeight * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pixels[(int)y * textureWidth + (int)x] = new Color(sample, sample, sample);
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    public PerlinNoise(int width, int height, float xOrigin, float yOrigin, float scale)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        this.xOrigin = xOrigin;
        this.yOrigin = yOrigin;
        this.scale = scale;
        texture = new Texture2D(textureWidth, textureHeight);
        pixels = new Color[textureWidth * textureHeight];
    }
    public PerlinNoise(int width, int height, float xOrigin, float yOrigin)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        this.xOrigin = xOrigin;
        this.yOrigin = yOrigin;
        texture = new Texture2D(textureWidth, textureHeight);
        pixels = new Color[textureWidth * textureHeight];
    }
    public PerlinNoise(int width, int height)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        texture = new Texture2D(textureWidth, textureHeight);
        pixels = new Color[textureWidth * textureHeight];
    }
}
