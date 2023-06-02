using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Booster<T>
{
    bool isClassified(T item);
}
