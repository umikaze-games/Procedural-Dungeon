using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TextureArrayContainer))]
public class TextureArrayContainerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.BeginHorizontal();

        // Get the SerializedProperties
        var texturesProp = property.FindPropertyRelative("textures");
        var selectedIndexProp = property.FindPropertyRelative("selectedTextureIndex");

        int arraySize = texturesProp.arraySize;

        if (arraySize < 2)
        {
            // Display a label when there are fewer than 2 textures
            EditorGUILayout.LabelField(label);
            EditorGUILayout.LabelField("No data");
        } else
        {   
            // Draw the IntSlider for selecting the index
            selectedIndexProp.intValue = EditorGUILayout.IntSlider(label.text, selectedIndexProp.intValue, 0, property.FindPropertyRelative("textures").arraySize - 1);
        }        

        EditorGUILayout.EndHorizontal();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // You can adjust the height if needed
        return EditorGUIUtility.singleLineHeight;
    }
}

