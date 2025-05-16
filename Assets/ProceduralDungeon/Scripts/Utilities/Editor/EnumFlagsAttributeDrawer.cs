using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    const float mininumButtonWidth = 80.0f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var enumWidth = EditorGUIUtility.currentViewWidth;
        var enumLength = property.enumNames.Length;
        var numColumns = Mathf.FloorToInt(enumWidth / mininumButtonWidth);
        var numRows = Mathf.CeilToInt((float)enumLength / numColumns);

        int buttonsIntValue = 0;
        bool[] buttonPressed = new bool[enumLength];
        float buttonWidth = (enumWidth - EditorGUIUtility.standardVerticalSpacing * (numColumns - 1)) / Mathf.Min(numColumns, enumLength);

        EditorGUILayout.LabelField(label);

        EditorGUI.BeginChangeCheck();
        
        for (int row = 0; row < numRows; row++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int column = 0; column < numColumns; column++)
            {
                int i = column + row * numColumns;

                if (i >= enumLength)
                {
                    GUILayout.Space(buttonWidth);
                    break;
                }

                // Check if the button is/was pressed
                if ((property.intValue & (1 << i)) == 1 << i)
                {
                    buttonPressed[i] = true;
                }

                buttonPressed[i] = GUILayout.Toggle(buttonPressed[i], property.enumNames[i], EditorStyles.miniButton, GUILayout.Width(buttonWidth));

                if (buttonPressed[i])
                    buttonsIntValue += 1 << i;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (EditorGUI.EndChangeCheck())
        {
            property.intValue = buttonsIntValue;
        }
    }
}
