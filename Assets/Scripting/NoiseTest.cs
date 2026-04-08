using UnityEngine;

public class NoiseTest : MonoBehaviour
{
    public int textureWidth = 512;
    public int textureHeight = 512;
    public float xOrigin;
    public float yOrigin;
    public float scale = 1.0f;
    public Noise noise;

    void Start()
    {
        noise = new PerlinNoise(textureWidth, textureHeight, xOrigin, yOrigin, scale);
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(noise.texture, new Rect(0.0f, 0.0f, noise.textureWidth, noise.textureHeight), new Vector2(0.5f, 0.5f));
    }
    void Update()
    {
        noise.textureWidth = textureWidth;
        noise.textureHeight = textureHeight;
        noise.xOrigin = xOrigin;
        noise.yOrigin = yOrigin;
        noise.scale = scale;
        noise.CalculateTexture();
    }
}
