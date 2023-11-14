using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public enum  BlackboardType
{
    Int,
    Float,
    Bool,
    Vector3,
    GameObject
}

[Serializable]
public class BlackboardEntry
{
    [SerializeField] string keyName;
    [SerializeField] object value;
    [SerializeField] string runtimeValue;
    [SerializeField] BlackboardType type;

    public string GetKeyName() { return keyName; }

    public BlackboardEntry(string keyname, BlackboardType type)
    {
        this.keyName = keyname;
        this.value = null;
        this.runtimeValue = "null";
        this.type = type;
    }
     
    public bool ClearEntryValue()
    {
        value = null;
        runtimeValue = "null";
        return true;
    }

    public bool SetValue<T>(T val)
    {
        if(val == null) //clear data if null
        {
            value = null;
            runtimeValue = "null";
            return true;
        }

        if(val.GetType() != typeDictionary[type])
        {
            Debug.LogError($"Trying to set blackboard data: {keyName}, with type {val.GetType()}, should be {typeDictionary[type]}");
            return false;
        }
        value = val;
        runtimeValue = val.ToString();
        return true;
    }

    public bool GetValue<T>(out T val)
    {
        val = default;
        
        if(value == null)
        {
            return false;
        }

        if(typeof(T) != typeDictionary[type])
        {
            return false;
        }
        val = (T)value; //casting because we have guaranteed that the type T is the type of the value member variable 
        return true;
    }

    static Dictionary<BlackboardType, System.Type> typeDictionary = new Dictionary<BlackboardType, Type>
    {
        { BlackboardType.Float, typeof(float) },
        { BlackboardType.Int, typeof(int) },
        { BlackboardType.Bool, typeof(bool) },
        { BlackboardType.Vector3, typeof(Vector3) },
        { BlackboardType.GameObject, typeof(GameObject) }

    };

    internal object GetRawValue()
    {
        return value;
    }
}
[CreateAssetMenu(menuName = "BehaviorTree/Blackboard")]
public class Blackboard : ScriptableObject
{
    [SerializeField] List<BlackboardEntry> blackboardData;

    public delegate void OnBlackboardValueChanged(BlackboardEntry entry);
    public event OnBlackboardValueChanged onblackboardValueChanged;

    public bool SetBlackboardData<T>(string keyname, T value)
    {
        foreach(var entry in blackboardData)
        {
            if(entry.GetKeyName() == keyname)
            { 
                if(entry.SetValue(value))
                {
                    onblackboardValueChanged?.Invoke(entry);
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        return false;
    }

    public bool GetBlackboardData<T>(string keyname, out T value)
    {
        foreach(var entry in blackboardData)
        {
            if(entry.GetKeyName() == keyname)
            {
               return entry.GetValue(out value);
            }
        }
        value = default;
        return false;
    }

    internal object GetBlackboardDataRawData(string keyName)
    {
        foreach (var entry in blackboardData)
        {
            if (entry.GetKeyName() == keyName)
            {
                return entry.GetRawValue();
            }
        }
        return null;
    }

    public void ClearBlackboardData(string keyName)
    {
        foreach (var entry in blackboardData)
        {
            if (entry.GetKeyName() == keyName)
            {
                entry.ClearEntryValue();
            }
        }

    }
}
