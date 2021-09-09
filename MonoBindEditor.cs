#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MonoBind))]
public class MonoBindDrawer : PropertyDrawer
{
    MonoBindPoint bindPoint;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // EditorGUI.BeginProperty(position, label, property);
        // EditorGUI.EndProperty();

        Rect nameRect = position;
        nameRect.width = position.width * 0.5f;
        EditorGUI.LabelField(nameRect, property.displayName + $" ({fieldInfo.FieldType.Name})");

        Rect hasBindRect = position;
        hasBindRect.position += Vector2.right * nameRect.width;
        hasBindRect.width = EditorGUIUtility.singleLineHeight;

        if (property.objectReferenceValue != null)
        {
            Color oldColor = GUI.color;
            GUI.color = Color.green;
            if (GUI.Button(hasBindRect, EditorGUIUtility.IconContent("Toolbar Plus").image))
            {
                Selection.activeObject = property.objectReferenceValue;
            }
            GUI.color = oldColor;
        }
        else
        {
            GUI.DrawTexture(hasBindRect, EditorGUIUtility.IconContent("Toolbar Minus").image);
        }

        System.Type objectType = fieldInfo.FieldType;

        var go = ((MonoBehaviour)property.serializedObject.targetObject).gameObject;

        var onCurrent = go.GetComponents(objectType);
        var onParent = go.GetComponentsInParent(objectType);
        var onChildren = go.GetComponentsInChildren(objectType);

        var selectionMenu = new GenericMenu();
        foreach (var comp in onCurrent)
        {
            selectionMenu.AddItem(new GUIContent("Current/ " + comp.name), false, () => { property.objectReferenceValue = comp; SaveProperty(property); });
        }

        foreach (var comp in onParent)
        {
            selectionMenu.AddItem(new GUIContent("Parent/ " + comp.name), false, () => { property.objectReferenceValue = comp; SaveProperty(property); });
        }

        foreach (var comp in onChildren)
        {
            selectionMenu.AddItem(new GUIContent("Children/ " + comp.name), false, () => { property.objectReferenceValue = comp; SaveProperty(property); });
        }

        selectionMenu.AddItem(new GUIContent("None"), false, () => { property.objectReferenceValue = null; SaveProperty(property); });

        Rect bindPointRect = position;
        bindPointRect.position += Vector2.right * (hasBindRect.width + nameRect.width);
        bindPointRect.width -= hasBindRect.width + nameRect.width;

        if (GUI.Button(bindPointRect, "Bind"))
        {
            selectionMenu.ShowAsContext();
        }
    }

    static void SaveProperty(SerializedProperty property)
    {
        property.serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(property.serializedObject.targetObject);
        AssetDatabase.SaveAssets();
    }
}

#endif