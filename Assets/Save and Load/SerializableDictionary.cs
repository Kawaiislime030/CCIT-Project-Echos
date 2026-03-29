using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    // 添加键值对
    public void Add(TKey key, TValue value)
    {
        if (ContainsKey(key))
        {
            int index = keys.IndexOf(key);
            values[index] = value;
        }
        else
        {
            keys.Add(key);
            values.Add(value);
        }
    }

    // 移除
    public bool Remove(TKey key)
    {
        int index = keys.IndexOf(key);
        if (index >= 0)
        {
            keys.RemoveAt(index);
            values.RemoveAt(index);
            return true;
        }
        return false;
    }

    // 判断是否包含
    public bool ContainsKey(TKey key) => keys.Contains(key);

    // 索引器
    public TValue this[TKey key]
    {
        get
        {
            int index = keys.IndexOf(key);
            if (index >= 0)
                return values[index];
            throw new KeyNotFoundException($"Key {key} not found.");
        }
        set
        {
            int index = keys.IndexOf(key);
            if (index >= 0)
                values[index] = value;
            else
                Add(key, value);
        }
    }

    // 清空
    public void Clear()
    {
        keys.Clear();
        values.Clear();
    }

    // 转换成标准 Dictionary
    public Dictionary<TKey, TValue> ToDictionary()
    {
        Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
        for (int i = 0; i < keys.Count; i++)
        {
            dict[keys[i]] = values[i];
        }
        return dict;
    }

    // 支持 foreach
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            yield return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
