using UnityEngine;

public class PerlinNoise : Noise
{
    private Vector2[,] gradientVectors = new Vector2[256,256]; //array with the gradientvectors the noise is based on

    //method that generates random gradientvectors
    private void generateGradientVectors()
    {
        //array gets filled with random vectors
        for (int i = 0; i < 256; i++)
        {
            for (int j = 0; j < 256; j++)
            {
                float random = Random.Range(0f, 2 * Mathf.PI);
                gradientVectors[i,j] = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
            }
        }

        //array gets shuffled
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

    public override float NoiseFunction(float xCoord, float yCoord)
    {
        int X = (int)Mathf.Floor(xCoord) & 255;
        int Y = (int)Mathf.Floor(yCoord) & 255;
        float xf = xCoord - Mathf.Floor(xCoord);
        float yf = yCoord - Mathf.Floor(yCoord);

        //the gradient vectors for each corner are stored in a variable for easy access.
        Vector2 gradienttopright = gradientVectors[(int)X+1, (int)Y+1];
        Vector2 gradienttopleft = gradientVectors[(int)X, (int)Y+1];
        Vector2 gradientbottomleft = gradientVectors[(int)X,(int)Y];
        Vector2 gradientbottomright = gradientVectors[(int)X+1, (int)Y];

        //the vectors from each corner to the sample point are stored
        Vector2 topright = new Vector2(xf-1, yf-1);
        Vector2 topleft = new Vector2(xf, yf-1);
        Vector2 bottomleft = new Vector2(xf, yf);
        Vector2 bottomright = new Vector2(xf-1, yf);
        
        //the dot products of each corner are calculated
        float dottopleft = (gradienttopleft.x * topleft.x) + (gradienttopleft.y * topleft.y);
        float dottopright = (gradienttopright.x * topright.x) + (gradienttopright.y * topright.y);
        float dotbottomleft = (gradientbottomleft.x * bottomleft.x) + (gradientbottomleft.y * bottomleft.y);
        float dotbottomright = (gradientbottomright.x * bottomright.x) + (gradientbottomright.y * bottomright.y);

        //bilinear interpolation between the four dot products calculated earlier
        float output = Mathf.SmoothStep(Mathf.SmoothStep(dotbottomleft, dottopleft, yf), Mathf.SmoothStep(dotbottomright, dottopright, yf), xf);

        //maps the output on to the range [0, 1.0] from the range [-1, 1]
        return (output + 1.0f) / 2.0f;
    }

    //Constructors with differing amounts of detail provided
    public PerlinNoise(int width, int height, float xOrigin, float yOrigin, float scale) : base(width, height, xOrigin, yOrigin, scale)
    {
        generateGradientVectors(); //runs gradient vector generation
    }
    public PerlinNoise(int width, int height, float xOrigin, float yOrigin) : base(width, height, xOrigin, yOrigin)
    {
        generateGradientVectors();
    }
    public PerlinNoise(int width, int height) : base(width, height)
    {
        generateGradientVectors();
    }
}
