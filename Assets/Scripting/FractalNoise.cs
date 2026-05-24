using UnityEngine;

public class FractalNoise : Noise
{
    private static int layers = 32;
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
            float xCoord = xOrigin + x / textureWidth * noiseArray[i].getScale();
            float yCoord = yOrigin + y / textureHeight * noiseArray[i].getScale();
            sample += noiseArray[i].NoiseFunction(xCoord,yCoord) / (Mathf.Pow(i + 1, 1.75f) + 5);
        }
        return sample;
    }

    private void fillNoiseArray()
    {
        for (int i = 0; i < layers; i++)
        {
            noiseArray[i] = new PerlinNoise(textureWidth, textureHeight, seed, xOrigin, yOrigin, scale * (i + 1));
            if(seed != null) seed += 1;
        }
    }

    //for testing purposes
    public override void updateScale(float newScale){
        this.scale = newScale;
        for(int i = 0; i < layers; i++){
                noiseArray[i].updateScale(newScale * (i + 1));
        }
    }

    public override string ToString(){
	string output = "Type: "+this.GetType()+", seed: "+seed+", scale: "+scale+"\n";
	for(int i = 0; i < layers; i++){
	    float strength = 1/(Mathf.Pow(i + 1, 1.75f) + 5);
	    output = output+"Layer "+i+": scale: "+noiseArray[i].getScale()+", Strength: "+strength+"\n";
	}
	return output;
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
