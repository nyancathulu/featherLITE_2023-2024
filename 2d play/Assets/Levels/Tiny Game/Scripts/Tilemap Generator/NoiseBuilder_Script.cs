using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Mathematics;

public class NoiseBuilder_Script : MonoBehaviour
{
    public float noiseScale;

    public float textureWidth;
    private float textureHeight;

    public float gridWidth;
    private float gridHeight;


    private float offsetX;

    private float offsetY;

    Texture2D NoiseTexture;

    [Range(0, 1)]
    public float tilePlaceThreshold;

    [Range(0, 1)]
    public float middleBarPercentage;

    public Tilemap fortressTilemap;
    public TileBase fortressTile;
    

    // Start is called before the first frame update
    void Start()
    {
        textureHeight = textureWidth * 0.75f;
        gridHeight = gridWidth * 0.75f;

       

       

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            NoiseTexture = GenerateNoiseTexture(textureWidth, textureHeight);

            GetComponent<MeshRenderer>().material.mainTexture = NoiseTexture;

            ApplyTiles(NoiseTexture, gridWidth, gridHeight);
        }
        
    }


    Texture2D GenerateNoiseTexture(float width, float height)
    {
        offsetX = UnityEngine.Random.Range(0f, 999999f);

        offsetY = UnityEngine.Random.Range(0f, 999999f);

        Texture2D Noise_texture = new Texture2D((int)width, (int)height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Noise_texture.SetPixel(x, y, CalculateNoiseColor(x, y));
            }
        }
        Noise_texture.Apply();
        return Noise_texture;
    }

    Color CalculateNoiseColor(int x, int y)
    {
        float xCoord = (float)x / textureWidth * noiseScale + offsetX;
        float yCoord = (float)y / textureHeight * noiseScale + offsetY;

        float sample = noise.snoise(new float2(xCoord, yCoord));
        
        return new Color(sample, sample, sample);
    }


    void ApplyTiles(Texture2D Noise_Texture, float grid_width, float grid_height)
    {
        fortressTilemap.ClearAllTiles();
        for (float x = 0; x < grid_width; x++)
        {
            for (float y = 0; y < grid_height; y++)
            {
                Color pixel_color = Noise_Texture.GetPixel((int)(textureWidth * (x / grid_width)), (int)(textureHeight * (y / grid_height)));

                float G_pixel_color = pixel_color.r;

                if (G_pixel_color >= tilePlaceThreshold)
                {
                    fortressTilemap.SetTile(new Vector3Int((int)x-32, (int)y-24, 0), fortressTile);
                }
            }
        }
        ApplyMiddleBar(middleBarPercentage);
    }

    void ApplyMiddleBar(float percentage)
    {
        int middlebarRange = Mathf.FloorToInt((gridWidth * middleBarPercentage)/2);
        for (int x = -middlebarRange; x <= middlebarRange; x++)
        {
            for (float y = 0; y < gridHeight; y++)
            {
               fortressTilemap.SetTile(new Vector3Int(x, (int)y - 24, 0), null);
            }
        }
    }
}