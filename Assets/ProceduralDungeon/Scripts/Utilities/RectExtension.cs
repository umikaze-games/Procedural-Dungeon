using UnityEngine;

public static class RectExtension
{
    /// <summary>
    /// Returns true if the given rect is completely inside the other rect.
    /// </summary>
    /// <param name="rect">The container rectangle.</param>
    /// <param name="other">The other rectangle. Checks if this rectangle is contained in the container rectangle.</param>
    /// <returns></returns>
    public static bool Contains(this Rect rect, Rect other)
    {
        // Bottom left corner
        var p0 = new Vector2(other.x, other.y);
        // Top right corner
        var p1 = new Vector2(other.x + other.width, other.y + other.height);
        return rect.Contains(p0) && rect.Contains(p1);
    }
}
