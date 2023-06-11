using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove
{
    private readonly IPlayerInput playerInput;
    public Vector3 vector;
    private readonly PlayerSettings playerSettings;
    private readonly Animator animator;

    public PlayerMove(IPlayerInput playerInput, Vector3 vector, PlayerSettings playerSettings, Animator animator)
    {
        this.playerInput = playerInput;
        this.vector = vector;
        this.playerSettings = playerSettings;
        this.animator = animator;
    }

    public void Tick(CharacterController characterController, Gravity gravity)
    {
        //vector = Vector3.zero; // обнуление направления
        vector.x = - playerInput.Horizontal * playerSettings.Speed; //направление по горизонтали
        vector.z = - playerInput.Verical * playerSettings.Speed; // направление по вертикали
        vector.y = gravity.gravityMovement.y;
        characterController.Move((vector * Time.deltaTime)); // Передвиженеи по направлению
    }
}
