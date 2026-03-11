using UnityEngine;

public class NoiseGeneration : MonoBehaviour
{ 
    public int textureWidth = 512;
    public int textureHeight = 512;
    public float xOrigin;
    public float yOrigin;

    public float scale = 1.0f;

    private Texture2D texture;
    private Color[] pixels;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        texture = new Texture2D(textureWidth, textureHeight);
        pixels = new Color[textureWidth * textureHeight];
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, textureWidth, textureHeight), new Vector2(0.5f, 0.5f));

    }

    void CalculateTexture(){
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
    }

    // Update is called once per frame
    void Update()
    {
        CalculateTexture();
    }
}