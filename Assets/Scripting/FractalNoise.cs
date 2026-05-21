using UnityEngine;

public class FractalNoise : Noise
{
    private static int layers = 4;
    private Noise[] noiseArray = new Noise[layers];
    

    //Method that calculates the texture
    public override Texture2D CalculateTexture(){
        for (float y = 0.0f; y < textureHeight; y++){
            for (float x = 0.0f; x < textureWidth; x++) {
                float sample = NoiseFunction(x,y);
                //Stores the sample in the pixels array.
                pixels[(int)y * textureWidth + (int)x] = new Color(sample, sample, sample);
                heights[(int)x,(int)y] = sample;
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }


    public override float NoiseFunction(float x, float y)
    {
        //Stores the output of the noise function in a variable
        float sample = 0;
        for (int i = 0; i < layers; i++)
        {
            //Calculates the coordinates for where the noise is sampled.
            float xCoord = xOrigin + x / textureWidth * scale * (i + 1);
            float yCoord = yOrigin + y / textureHeight * scale * (i + 1);
            sample = sample + (noiseArray[i].NoiseFunction(xCoord,yCoord) / (i + 1));
        }
        sample = sample / layers;
        return sample;
    }

    private void fillNoiseArray()
    {
        for (int i = 0; i < layers; i++)
        {
            noiseArray[i] = new PerlinNoise(textureWidth, textureHeight, seed, xOrigin, yOrigin, scale * i);
        }
    }

    //for testing purposes
    public override void updateScale(float newScale){
        this.scale = newScale;
        foreach (Noise noise in noiseArray)
            {
                noise.updateScale(newScale);
            }
        }

    //Constructors
    public FractalNoise(int width, int height, int? seed, float xOrigin, float yOrigin, float scale) : base(width, height, seed, xOrigin, yOrigin, scale)
    {
        fillNoiseArray();
    }
    public FractalNoise(int width, int height, int? seed, float xOrigin, float yOrigin) : base(width, height, seed, xOrigin, yOrigin)
    {
        fillNoiseArray();
    }
    public FractalNoise(int width, int height, int? seed) : base(width, height, seed)
    {
        fillNoiseArray();
    }
    public FractalNoise(int width, int height) : base(width, height)
    {
        fillNoiseArray();
    }
}
