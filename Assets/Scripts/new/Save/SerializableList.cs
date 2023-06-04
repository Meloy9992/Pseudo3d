using Palmmedia.ReportGenerator.Core.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[Serializable]
public class SerializableListChunk<Chunk> : List<Chunk>, ISerializationCallbackReceiver // TValue - принимающий тип. 
                                                                                     // List<TValue> - Наследование от List
{
    [SerializeField] private List<Chunk> chunks = new List<Chunk>(); // Создаем сериализованный список значений
    [SerializeField] private List<Chunk> _myList = new List<Chunk>(); // Создаем сериализованный список значений


    public void OnBeforeSerialize()
    {
        // оЧИСТИТЬ ЗНАЧЕНИЯ
        chunks.Clear();
        foreach (var kvp in chunks)
        {
            chunks.Add(kvp);
        }

    }

    public void OnAfterDeserialize() // Получение объекта после сериализации unity
    {
        //this.Clear(); // Очистить словарь

        _myList = new List<Chunk>();

        for (int i = 0; i < chunks.Count; i++)
        {
            _myList.Add(chunks[i]); // Добавить значения
        }
    }
}
