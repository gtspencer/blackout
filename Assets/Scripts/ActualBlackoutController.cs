using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualBlackoutController : MonoBehaviour
{
    public int textureWidth = 1920;
    public int textureHeight = 1080;
    public Color textureColor;
    private Texture2D texture;

    private List<Tuple<int, int>> elements;
    private Queue<Tuple<int, int>> queue;

    public int boxWidth = 5;
    public int boxHeight = 9;

    void Start()
    {
        textureColor = new Color(0, 0, 0, 0);
        elements = new List<Tuple<int, int>>();
        
        // Step 1: Create a new GameObject
        GameObject textureObject = new GameObject("TextureObject");

        // Step 2: Add a SpriteRenderer to the GameObject
        SpriteRenderer spriteRenderer = textureObject.AddComponent<SpriteRenderer>();

        // Step 3: Create a Texture2D and set it to a single color
        texture = new Texture2D(textureWidth, textureHeight);
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                texture.SetPixel(x, y, textureColor);
            }
        }
        
        for (int x = 0; x < textureWidth; x+=boxWidth)
        {
            for (int y = 0; y < textureHeight; y+=boxHeight)
            {
                elements.Add(new Tuple<int, int>(x, y));
            }
        }
        
        texture.Apply();
        
        elements.Shuffle();
        queue = new Queue<Tuple<int, int>>();
        foreach (Tuple<int, int> t in elements)
        {
            queue.Enqueue(t);
        }

        // Create a Sprite from the Texture2D
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, textureWidth, textureHeight), new Vector2(0.5f, 0.5f));

        // Set the Sprite to the SpriteRenderer
        spriteRenderer.sprite = sprite;
        
        spriteRenderer.material.shader = Shader.Find("Unlit/SpriteZBuffer");

        // Step 4: Position the GameObject in front of the camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Set the position in front of the camera
            textureObject.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 5.0f;
            
            // Step 5: Scale the GameObject to fit the camera view
            float height = 2f * mainCamera.orthographicSize;
            float width = height * mainCamera.aspect;

            textureObject.transform.localScale = new Vector3(width / sprite.bounds.size.x, height / sprite.bounds.size.y, 1);
            
            textureObject.transform.SetParent(mainCamera.transform);

            textureObject.transform.localPosition = new Vector3(textureObject.transform.localPosition.x,
                textureObject.transform.localPosition.y, 8.5f);
        }

        var count = boxHeight * boxWidth;
        colors = new Color32[count];
        for (int i = 0; i < count; i++)
        {
            colors[i] = Color.black;
        }

        // started = true;
    }


    private bool done = false;
    private Color32[] colors;
    private void FixedUpdate()
    {
        if (done) return;
        // if (!started) return;
        if (queue.TryDequeue(out var result))
        {
            try
            {
                texture.SetPixels32(result.Item1, result.Item2, boxWidth, boxHeight, colors);
                texture.Apply();
            }
            catch
            {
                
            }
        }
        else
        {
            done = true;
        }
    }
}
