using UnityEngine;

public class PerlinNoise : Noise
{
    private Color[] pixels;
    private Vector2[,] gradientVectors = new Vector2[256,256];

    private void generateGradientVectors()
    {
        for (int i = 0; i < 256; i++)
        {
            for (int j = 0; j < 256; j++)
            {
                //float random = Random.Range(0f, 2 * Mathf.PI);
                //gradientVectors[i,j] = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
                gradientVectors[i,j] = new Vector2(i,j);
                gradientVectors[i,j] = gradientVectors[i,j].normalized;
            }
        }

        for (int i = 0; i < 256; i++)
        {
            for (int j = 0; j < 256; j++)
            {
                int randi = Random.Range(0, 256);
                int randj = Random.Range(0, 256);

                Vector2 vectorstash = gradientVectors[randi,randj];
                gradientVectors[randi,randj] = gradientVectors[i,j];
                gradientVectors[i,j] = vectorstash;
            }
        }
    }

    public override Texture2D CalculateTexture(){
        for (float y = 0.0f; y < textureHeight; y++){
            for (float x = 0.0f; x < textureWidth; x++) {
                float xCoord = xOrigin + x / textureWidth * scale;
                float yCoord = yOrigin + y / textureHeight * scale;
                //float sample = Mathf.PerlinNoise(xCoord, yCoord);
                float sample = NoiseFunction(xCoord, yCoord);
                pixels[(int)y * textureWidth + (int)x] = new Color(sample, sample, sample);
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    private float NoiseFunction(float xCoord, float yCoord)
    {
        int X = (int)Mathf.Floor(xCoord) & 255;
        int Y = (int)Mathf.Floor(yCoord) & 255;
        float xf = xCoord - Mathf.Floor(xCoord);
        float yf = yCoord - Mathf.Floor(yCoord);

        Vector2 gradienttopright = gradientVectors[(int)X+1, (int)Y+1];
        Vector2 gradienttopleft = gradientVectors[(int)X, (int)Y+1];
        Vector2 gradientbottomleft = gradientVectors[(int)X,(int)Y];
        Vector2 gradientbottomright = gradientVectors[(int)X+1, (int)Y];

        Vector2 topright = new Vector2(xf-1, yf-1);
        Vector2 topleft = new Vector2(xf, yf-1);
        Vector2 bottomleft = new Vector2(xf, yf);
        Vector2 bottomright = new Vector2(xf-1, yf);
        
        float dottopleft = (gradienttopleft.x * topleft.x) + (gradienttopleft.y * topleft.y);
        float dottopright = (gradienttopright.x * topright.x) + (gradienttopright.y * topright.y);
        float dotbottomleft = (gradientbottomleft.x * bottomleft.x) + (gradientbottomleft.y * bottomleft.y);
        float dotbottomright = (gradientbottomright.x * bottomright.x) + (gradientbottomright.y * bottomright.y);

        return Mathf.Lerp(Mathf.Lerp(dotbottomleft, dottopleft, yf), Mathf.Lerp(dotbottomright, dottopright, yf), xf);
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
        generateGradientVectors();
    }
    public PerlinNoise(int width, int height, float xOrigin, float yOrigin)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        this.xOrigin = xOrigin;
        this.yOrigin = yOrigin;
        texture = new Texture2D(textureWidth, textureHeight);
        pixels = new Color[textureWidth * textureHeight];
        generateGradientVectors();
    }
    public PerlinNoise(int width, int height)
    {
        this.textureWidth = width;
        this.textureHeight = height;
        texture = new Texture2D(textureWidth, textureHeight);
        pixels = new Color[textureWidth * textureHeight];
        generateGradientVectors();
    }
}
