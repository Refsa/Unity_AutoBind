using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Attribute = System.Attribute;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
public sealed class AutoBindAttribute : Attribute
{
    public bool BindChildren = false;

    public AutoBindAttribute() { }
    public AutoBindAttribute(bool bindChildren)
    {
        BindChildren = bindChildren;
    }
}

public static class AutoBindComponent
{
    static IEnumerable<FieldInfo> GetAutoBindFields(object target)
    {
        return target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(e => Attribute.IsDefined(e, typeof(AutoBindAttribute)));
    }

    public static void AutoBind(GameObject target)
    {
        if (target.GetType().GetCustomAttribute<AutoBindAttribute>() is AutoBindAttribute attr)
        {
            if (attr.BindChildren)
            {
                foreach (Transform child in target.transform)
                {
                    AutoBind(child.gameObject);
                }
            }
        }

        foreach (MonoBehaviour obj in target.GetComponents<MonoBehaviour>())
        {
            AutoBind(obj, target);
        }
    }

    public static void AutoBind(object target, GameObject owner)
    {
        var fields = GetAutoBindFields(target);

        foreach (var field in fields)
        {
            var comp = owner.GetComponent(field.FieldType);
            if (comp != null)
            {
                field.SetValue(target, comp);
            }
            else
            {
                UnityEngine.Debug.LogError($"AutoBind: Could not find {field.FieldType} on GameObject {owner.name}", owner);
            }
        }
    }
}