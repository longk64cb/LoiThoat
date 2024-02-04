using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rgBody;
    [SerializeField] private Animator animator;

    [SerializeField] private float speed = 2f;

    [SerializeField] private Vector2 verticalVector = new Vector2(-0.5f, 0.25f);
    [SerializeField] private Vector2 horizontalVector = new Vector2(0.5f, 0.25f);
    private Vector2 moveDirection;

    private void FixedUpdate()
    {
        moveDirection = Vector2.zero;

        float horz = Input.GetAxisRaw("Horizontal");
        //if (horz != 0f)
        //{
        //    Move(horz, 0f);
        //    return;
        //}
        float vert = Input.GetAxisRaw("Vertical");
            Move(horz, vert);
    }

    public void Move(float horz, float vert)
    {
        var horzVelocity = horz * horizontalVector.normalized;
        var vertVelocity = vert * verticalVector.normalized;

        var finalVelocity = (horzVelocity + vertVelocity).normalized;
        rgBody.velocity = finalVelocity * speed;
        MoveAnimation(horz, vert);
    }

    public void MoveAnimation(float horz, float vert)
    {
        if (horz == 0f &&  vert == 0f)
        {
            animator.SetFloat("speed", 0);
        }
        else
        {
            animator.SetFloat("speed", 1);
            animator.SetFloat("horz", horz);
            animator.SetFloat("vert", vert);
        }
    }

    [ContextMenu("Normalized")]
    public void Normalized()
    {
        Debug.Log(new Vector2(9.73f, -5.93f).normalized);
    }
}
