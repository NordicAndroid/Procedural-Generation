using UnityEngine;

public abstract class Noise
{
    public int textureWidth = 512;
    public int textureHeight = 512;
    public float xOrigin;
    public float yOrigin;
    public float scale = 1.0f;

    public Texture2D texture;
    public abstract Texture2D CalculateTexture();
}
