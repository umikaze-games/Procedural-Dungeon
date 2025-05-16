using System;
using UnityEngine;

[Serializable]
public class Array2DWrapper<T>
{
    [SerializeField] T[] array;
    [SerializeField, Delayed] int width;
    [SerializeField, Delayed] int height;

    public Array2DWrapper(int width, int height)
    {
        this.width = width;
        this.height = height;
        array = new T[width * height];
    }

    public T this[int x, int y]
    {
        get => array[x + y * width];
        set => array[x + y * width] = value;
    }

    // Helper function to convert (x, y) indices to a single index in the 1D array
    internal int ConvertToIndex(int x, int y)
    {
        return y * width + x;
    }

    // Helper function to convert 1D index to 2D coordinates (row, column)
    Vector2Int ConvertToCoordinates(int index, int width)
    {
        var x = index / width;
        var y = index % width;
        return new Vector2Int(x, y);
    }

    public int Width => width;
    public int Height => height;
    // FIXME: This should not be public, remove entirely
    public T[] Array => array;

    public int Length { get => array.Length; }
}
