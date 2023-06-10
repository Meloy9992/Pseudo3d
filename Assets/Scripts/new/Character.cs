using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public CharacterController characterController; // ���������� ���������

    public bool facingRight; // �������� �������

    protected virtual void Move() {
    }

    protected virtual void Attack() { }

    public virtual void Flip()
    {

        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;// �������� �� x
        transform.localScale = scaler;
    }
    public bool isGrounded()
    {
        return characterController.isGrounded; // ���� �������� �� �����
    }
}
