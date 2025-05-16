using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextureArrayContainer
{
    [SerializeField] List<Texture2D> textures;
    [SerializeField] int selectedTextureIndex;

    public Texture2D SelectedTexture => textures.Count > selectedTextureIndex ? textures[selectedTextureIndex] : null;

    public TextureArrayContainer()
    {
        textures = new List<Texture2D>();
    }

    public void AddTexture(Texture2D texture)
    {
        textures.Add(texture);
    }

    public Texture2D GetTexture(int index)
    {
        if (index >= 0 && index < textures.Count)
        {
            return textures[index];
        }
        else
        {
            Debug.LogError("Index out of bounds.");
            return null;
        }
    }

    public void Clear()
    {
        textures.Clear();
        selectedTextureIndex = 0;
    }
}
