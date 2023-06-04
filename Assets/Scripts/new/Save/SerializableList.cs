using Palmmedia.ReportGenerator.Core.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[Serializable]
public class SerializableListChunk<Chunk> : List<Chunk>, ISerializationCallbackReceiver // TValue - ����������� ���. 
                                                                                     // List<TValue> - ������������ �� List
{
    [SerializeField] private List<Chunk> chunks = new List<Chunk>(); // ������� ��������������� ������ ��������
    [SerializeField] private List<Chunk> _myList = new List<Chunk>(); // ������� ��������������� ������ ��������


    public void OnBeforeSerialize()
    {
        // �������� ��������
        chunks.Clear();
        foreach (var kvp in chunks)
        {
            chunks.Add(kvp);
        }

    }

    public void OnAfterDeserialize() // ��������� ������� ����� ������������ unity
    {
        //this.Clear(); // �������� �������

        _myList = new List<Chunk>();

        for (int i = 0; i < chunks.Count; i++)
        {
            _myList.Add(chunks[i]); // �������� ��������
        }
    }
}
