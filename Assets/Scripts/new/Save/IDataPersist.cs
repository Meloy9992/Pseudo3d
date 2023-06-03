using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersist 
{
    void LoadData(DataGame data);

    void SaveData(ref DataGame data);
}
