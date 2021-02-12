using System.Reflection;
using System;


public static class ReflectionUtils
{
    public static T GetFieldValue<T>(object obj, string fieldName)
    {
        return (T)GetFieldInfo(obj, fieldName).GetValue(obj);
    }

    public static void SetFieldValue<T>(object obj, string fieldName, T value)
    {
        GetFieldInfo(obj, fieldName).SetValue(obj, value);
    }

    public static FieldInfo GetFieldInfo(object obj, string fieldName)
    {
        FieldInfo info = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (info == null)
        {
            throw new NullReferenceException($"no such field found {fieldName}");
        }
        return info;
    }
}
