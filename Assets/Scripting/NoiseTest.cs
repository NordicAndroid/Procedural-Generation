using UnityEngine;

public class NoiseTest : MonoBehaviour
{
    public int textureWidth = 512;
    public int textureHeight = 512;
    public float xOrigin = 0f;
    public float yOrigin = 0f;
    public float scale = 1.0f;
    public int seed = 17;
    public Noise noise;

    void Start()
    {
        noise = new FractalNoise(textureWidth, textureHeight, seed, xOrigin, yOrigin, scale);
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(noise.texture, new Rect(0.0f, 0.0f, textureWidth, textureHeight), new Vector2(0.5f, 0.5f));
        noise.CalculateTexture();
    }
}
