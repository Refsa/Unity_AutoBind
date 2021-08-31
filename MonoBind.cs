using System;
using UnityEngine;

public enum MonoBindPoint
{
    Current,
    FromParent,
    FromChildren,
}

[AttributeUsage(AttributeTargets.Field)]
public sealed class MonoBind : PropertyAttribute
{
    MonoBindPoint bindPoint;
    public MonoBindPoint BindPoint => bindPoint;

    public MonoBind(MonoBindPoint bindPoint)
    {
        this.bindPoint = bindPoint;
    }

    public MonoBind()
    {
        
    }
}