using UnityEngine;

public abstract class Noise
{
    public int textureWidth = 512;
    public int textureHeight = 512;
    public float xOrigin;
    public float yOrigin;
    public float scale = 1.0f;

    private Color[] pixels; //Array in which color values are stored in
    public Texture2D texture;
    public abstract Texture2D CalculateTexture();

    //Constructors
    public Noise(int width, int height, float xOrigin, float yOrigin, float scale)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        this.xOrigin = xOrigin;
        this.yOrigin = yOrigin;
        this.scale = scale;
        texture = new Texture2D(textureWidth, textureHeight); //initialize texture based on provided size
        pixels = new Color[textureWidth * textureHeight]; //initialize pixels array with the right length based on texture size
    }

    public Noise(int width, int height, float xOrigin, float yOrigin)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        this.xOrigin = xOrigin;
        this.yOrigin = yOrigin;
        texture = new Texture2D(textureWidth, textureHeight);
        pixels = new Color[textureWidth * textureHeight];
    }

    public Noise(int width, int height)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        texture = new Texture2D(textureWidth, textureHeight);
        pixels = new Color[textureWidth * textureHeight];
    }
}
