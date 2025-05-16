using UnityEngine;
using System.IO;
using UnityEditor;

public static class Texture2DExtension
{
    /// <summary>
    /// Save content of a texture into a png image file.
    /// </summary>
    /// <param name="texture">The 2D texture that will be saved.</param>
    /// <param name="relativeFolder">Folder name used relative to the Application data Path. If the folder does not exist, it will be created.</param>
    /// <param name="fileName">Name of the file without file ending. The file ending .png will be attached.</param>
    public static void Save(this Texture2D texture, string relativeFolder, string fileName)
    {
        char separator = Path.DirectorySeparatorChar;
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + separator + relativeFolder;
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        string filePath = dirPath + separator + fileName + ".png";
        Debug.Log("Texture was stored in: " + filePath);
        File.WriteAllBytes(filePath, bytes);
    }

    /// <summary>
    /// Takes an existing texture asset and saves it as png in the same place.
    /// </summary>
    /// <param name="texture">A texture that is already an asset in the project. If the texture is not an asset the method will fail.</param>
    public static void SaveAsset(this Texture2D texture)
    {
        var bytes = texture.EncodeToPNG();
        var assetPath = AssetDatabase.GetAssetPath(texture);
        File.WriteAllBytes(assetPath, bytes);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Save a Texture2D Object as Asset.
    /// </summary>
    /// <param name="texture">Any Texture2D object.</param>
    /// <param name="path">Path relative to the Assets folder. For example: "MyFolder/texture". This will save the texture in "Assets/MyFolder/texture.png"</param>
    public static void SaveAsAsset(this Texture2D texture, string path)
    {
        var bytes = texture.EncodeToPNG();
        var assetPath = "Assets/" + path + ".png";
        File.WriteAllBytes(assetPath, bytes);
        AssetDatabase.Refresh();
        var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        importer.isReadable = texture.isReadable;
        importer.mipmapEnabled = false;
        importer.SaveAndReimport();
    }

    public static Texture2D Copy(this Texture2D texture)
    {
        // Get the raw texture data from the original texture
        byte[] rawTextureData = texture.GetRawTextureData();

        // Create a new Texture2D using the raw texture data
        Texture2D copiedTexture = new Texture2D(texture.width, texture.height, texture.format, texture.mipmapCount > 1);

        // Load the raw texture data into the new texture
        copiedTexture.LoadRawTextureData(rawTextureData);

        // Apply changes to the texture
        copiedTexture.Apply();

        return copiedTexture;
    }

    public static void FillWithColor(this Texture2D texture, Color color)
    {
        texture.DrawRectangle(new RectInt(0, 0, texture.width, texture.height), color);
    }

    public static void DrawRectangle(this Texture2D texture, RectInt rect, Color color)
    {
        for (int y = rect.yMin; y < rect.yMax; y++)
        {
            for (int x = rect.xMin; x < rect.xMax; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
    }

    public static void DrawTexture(this Texture2D texture, Texture2D sourceTexture, RectInt destinationRect)
    {
        int startX = Mathf.Max(0, destinationRect.x);
        int startY = Mathf.Max(0, destinationRect.y);
        int endX = Mathf.Min(destinationRect.x + destinationRect.width, texture.width);
        int endY = Mathf.Min(destinationRect.y + destinationRect.height, texture.height);

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                Color sourceColor = sourceTexture.GetPixel(x - destinationRect.x, y - destinationRect.y);
                texture.SetPixel(x, y, sourceColor);
            }
        }

        texture.Apply();
    }

    /// <summary>
    /// Converts a Texture2D to black and white where every pixel is white unless it is already black in the original.
    /// </summary>
    /// <param name="original">The original Texture2D to convert.</param>
    public static void ConvertToBlackAndWhite(this Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i] != Color.black)
            {
                pixels[i] = Color.white;
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
    }

    public static void DrawLine(this Texture2D texture, Vector2Int p0, Vector2Int p1, Color color)
    {
        int dx = Mathf.Abs(p1.x - p0.x);
        int dy = Mathf.Abs(p1.y - p0.y);
        int sx = (p0.x < p1.x) ? 1 : -1;
        int sy = (p0.y < p1.y) ? 1 : -1;
        int err = dx - dy;

        while (p0 != p1)
        {
            texture.SetPixel(p0.x, p0.y, color);

            int e2 = 2 * err;

            if (e2 > -dy && e2 < dx)
            {
                texture.SetPixel(p0.x + sx, p0.y, color);
            }

            if (e2 > -dy)
            {
                err -= dy;
                p0.x += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                p0.y += sy;
            }
        }

        texture.SetPixel(p0.x, p0.y, color);
        texture.Apply();
    }

}
