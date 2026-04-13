using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Foundation.CMS;

[CustomPropertyDrawer(typeof(SubclassSelector))]
public class SubclassSelectorDrawer : PropertyDrawer
{
    private static Dictionary<Type, Type[]> _typeCache = new();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ManagedReference)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        var fieldType = GetFieldBaseType(property);
        var types = GetSubclasses(fieldType);

        var currentTypeName = GetCurrentTypeName(property);
        var currentIndex = Array.FindIndex(types, t => t?.FullName == currentTypeName);

        var names = new string[types.Length + 1];
        names[0] = "<null>";
        for (int i = 0; i < types.Length; i++)
            names[i + 1] = types[i].Name;

        var dropdownRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var newIndex = EditorGUI.Popup(dropdownRect, label.text, currentIndex + 1, names) - 1;

        if (newIndex != currentIndex)
        {
            property.managedReferenceValue = newIndex < 0
                ? null
                : Activator.CreateInstance(types[newIndex]);
        }

        if (property.managedReferenceValue != null)
        {
            var childRect = new Rect(
                position.x,
                position.y + EditorGUIUtility.singleLineHeight + 2,
                position.width,
                position.height - EditorGUIUtility.singleLineHeight - 2
            );
            EditorGUI.PropertyField(childRect, property, GUIContent.none, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var lineHeight = EditorGUIUtility.singleLineHeight;
        if (property.managedReferenceValue == null)
            return lineHeight;
        return lineHeight + 2 + EditorGUI.GetPropertyHeight(property, label, true);
    }

    private Type GetFieldBaseType(SerializedProperty property)
    {
        var typeName = property.managedReferenceFieldTypename;
        var parts = typeName.Split(' ');
        if (parts.Length != 2) return typeof(object);

        var assemblyName = parts[0];
        var className = parts[1];
        return Type.GetType($"{className}, {assemblyName}") ?? typeof(object);
    }

    private Type[] GetSubclasses(Type baseType)
    {
        if (_typeCache.TryGetValue(baseType, out var cached))
            return cached;

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => { try { return a.GetTypes(); } catch { return Array.Empty<Type>(); } })
            .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface && t != baseType)
            .ToArray();

        _typeCache[baseType] = types;
        return types;
    }

    private string GetCurrentTypeName(SerializedProperty property)
    {
        var fullTypeName = property.managedReferenceFullTypename;
        if (string.IsNullOrEmpty(fullTypeName)) return null;
        var parts = fullTypeName.Split(' ');
        return parts.Length == 2 ? parts[1] : null;
    }
}