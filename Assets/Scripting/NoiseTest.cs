using UnityEngine;

public class NoiseTest : MonoBehaviour
{
    public int textureWidth = 512;
    public int textureHeight = 512;
    public float xOrigin = 0f;
    public float yOrigin = 0f;
    public float scale = 1.0f;
    public Noise noise;

    void Start()
    {
        noise = new FractalNoise(textureWidth, textureHeight);
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(noise.texture, new Rect(0.0f, 0.0f, textureWidth, textureHeight), new Vector2(0.5f, 0.5f));
    }
    void Update()
    {
        noise.updateScale(scale);
        noise.updateOrigin(xOrigin, yOrigin);
        noise.CalculateTexture();
    }
}
