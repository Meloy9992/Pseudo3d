using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : IPlayerInput
{
    public void ReadInput()
    {
        Verical = Input.GetAxis("Vertical");
        Horizontal = Input.GetAxis("Horizontal");
    }

    public float Verical { get; private set; }

    public float Horizontal { get; private set; }



}
