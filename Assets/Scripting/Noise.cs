using UnityEngine;

public abstract class Noise
{
    protected int textureWidth = 512;
    protected int textureHeight = 512;
    protected float xOrigin;
    protected float yOrigin;
    protected float scale = 1.0f;
    protected int? seed;
    public virtual void updateScale(float newScale){this.scale = newScale;} //for testing, updates the scale of noise
    public virtual void updateOrigin(float xOrigin, float yOrigin){this.xOrigin = xOrigin; this.yOrigin = yOrigin;} //for testing, updates origin

    protected Color[] pixels; //Array in which color values are stored in
    public Texture2D texture; //Output in texture form
    public float[,] heights;

    //Method that calculates the texture
    public virtual Texture2D CalculateTexture(){
        for (float y = 0.0f; y < textureHeight; y++){
            for (float x = 0.0f; x < textureWidth; x++) {
                //Calculates the coordinates for where the noise is sampled.
                float xCoord = xOrigin + x / textureWidth * scale;
                float yCoord = yOrigin + y / textureHeight * scale;
                //Stores the output of the noise function in a variable
                float sample = NoiseFunction(xCoord, yCoord);
                //Stores the sample in the pixels array.
                pixels[(int)y * textureWidth + (int)x] = new Color(sample, sample, sample);
                heights[(int)x,(int)y] = sample;
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    public Texture2D CalculateIslandTexture(){
        this.CalculateTexture();
        for(int y = 0; y < textureHeight; y++){
            for(int x = 0; x < textureWidth; x++){
                Vector3 centerCoords = new Vector3(textureWidth / 2, textureHeight / 2);
                float distFromCenter = Mathf.Sqrt((centerCoords.x - x) * (centerCoords.x - x) + (centerCoords.y - y) * (centerCoords.y - y));
                float sample = heights[x,y] / (0.0000001f * Mathf.Pow(distFromCenter / (textureWidth / 500) , 3) + 1) ;
                heights[x,y] = sample;
                pixels[y * textureWidth + x] = new Color(sample, sample, sample);
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    public abstract float NoiseFunction(float xCoord, float yCoord);

    //Constructors
    protected Noise(int width, int height, int? seed, float xOrigin, float yOrigin, float scale)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        this.seed = seed;
        this.xOrigin = xOrigin;
        this.yOrigin = yOrigin;
        this.scale = scale;
        texture = new Texture2D(textureWidth, textureHeight); //initialize texture based on provided size
        pixels = new Color[textureWidth * textureHeight]; //initialize pixels array with the right length based on texture size
        heights = new float[textureWidth, textureHeight];
    }

    protected Noise(int width, int height, int? seed, float xOrigin, float yOrigin)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        this.seed = seed;
        this.xOrigin = xOrigin;
        this.yOrigin = yOrigin;
        texture = new Texture2D(textureWidth, textureHeight);
        pixels = new Color[textureWidth * textureHeight];
        heights = new float[textureWidth, textureHeight];
    }

    protected Noise(int width, int height, int? seed)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        this.seed = seed;
        texture = new Texture2D(textureWidth, textureHeight);
        pixels = new Color[textureWidth * textureHeight];
        heights = new float[textureWidth, textureHeight];
    }

    protected Noise(int width, int height)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        texture = new Texture2D(textureWidth, textureHeight);
        pixels = new Color[textureWidth * textureHeight];
        heights = new float[textureWidth, textureHeight];
    }
}
