using UnityEditor;
using UnityEngine;

//FIXME: Rename to sth. better or WORSE
[CustomPropertyDrawer(typeof(Array2DWrapper<TileType>))]
public class TileTypePropertyDrawer : Array2DWrapperPropertyDrawer<TileType> { }

public class Array2DWrapperPropertyDrawer<T> : PropertyDrawer
{
    const string ARRAY = "array";
    const string WIDTH = "width";
    const string HEIGHT = "height";

    SerializedProperty arrayProperty;
    SerializedProperty widthProperty;
    SerializedProperty heightProperty;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        arrayProperty = property.FindPropertyRelative(ARRAY);
        widthProperty = property.FindPropertyRelative(WIDTH);
        heightProperty = property.FindPropertyRelative(HEIGHT);

        var target = property.serializedObject.targetObject;
        int width = widthProperty.intValue;
        int height = heightProperty.intValue;
        int oldWidth = width;
        int oldHeight = height;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField(label);
        EditorGUILayout.PropertyField(widthProperty);
        EditorGUILayout.PropertyField(heightProperty);
        if (EditorGUI.EndChangeCheck())
        {
            // Ensure width and height are non-negative
            width = Mathf.Max(0, widthProperty.intValue);
            height = Mathf.Max(0, heightProperty.intValue);
            Undo.RegisterCompleteObjectUndo(target, "Change Pattern Size");
            ResizeArray(arrayProperty, width, height, oldWidth, oldHeight);

            EditorUtility.SetDirty(target);
            property.serializedObject.ApplyModifiedProperties();
        }

        DisplayArrayGrid(target, width, height);
        property.serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Display the tiles in a two-dimensional grid
    /// </summary>
    /// <param name="target"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void DisplayArrayGrid(Object target, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < width; x++)
            {
                //int index = x + y * width;
                EditorGUI.BeginChangeCheck();
                var enumProperty = arrayProperty.GetArrayElementAtIndex(x + y * width);
                EditorGUILayout.PropertyField(enumProperty, GUIContent.none);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Change Tile Type");
                    EditorUtility.SetDirty(target);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void ResizeArray(SerializedProperty arrayProperty, int newWidth, int newHeight, int oldWidth, int oldHeight)
    {
        int newSize = newWidth * newHeight;

        // Insert as many elements in the front of the array as newSize. Set the element of each to the default value.
        for (int i = 0; i < newSize; i++)
        {
            arrayProperty.InsertArrayElementAtIndex(0);
            // FIXME: this line is not generic
            arrayProperty.GetArrayElementAtIndex(0).enumValueIndex = (int) TileType.Empty;
        }

        for (int y = 0; y < Mathf.Min(newHeight, oldHeight); y++)
        {
            for (int x = 0; x < Mathf.Min(newWidth, oldWidth); x++)
            {
                {
                    int newIndex = x + y * newWidth;
                    int oldIndex = x + y * oldWidth + newSize;
                    arrayProperty.MoveArrayElement(oldIndex, newIndex);
                }
            }
        }
        arrayProperty.arraySize = newSize;
    }

}