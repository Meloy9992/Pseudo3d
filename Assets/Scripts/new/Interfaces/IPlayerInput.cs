using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput 
{
    void ReadInput();

    float Verical { get; }
    float Horizontal { get; }
}
