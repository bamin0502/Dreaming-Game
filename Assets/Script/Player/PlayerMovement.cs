using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("플레이어 캐릭터의 이동 속도 및 이동 설정")]
    public float moveSpeed = 5f;
    private Vector3 moveDirection;

    private Rigidbody rb;
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    private PlayerHealth playerHealth;
    
    [Tooltip("애니메이터의 파라미터 지정")]
    private static readonly int Move = Animator.StringToHash("IsMove");

    private Camera _camera;
    private Plane plane;
    void Start()
    {
        _camera = Camera.main;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth 컴포넌트를 찾을 수 없습니다.");
        }
        plane = new Plane(Vector3.up, Vector3.zero);
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        if(playerHealth.isDead) return;
        Vector2 input = value.ReadValue<Vector2>();
        moveDirection = new Vector3(input.x, 0, input.y);
    }

    void Update()
    {
        if (playerHealth.isDead) return;
        if (moveDirection != Vector3.zero)
        {
            animator.SetBool(Move, true);
        }
        else
        {
            animator.SetBool(Move, false);
        }
        
        
    }

    private void RotateTowardsCursor()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }
    }

    private void LateUpdate()
    {
        RotateTowardsCursor();
    }

    void FixedUpdate()
    {
        if (playerHealth.isDead) return;
        if (moveDirection != Vector3.zero)
        {
            Vector3 move = new Vector3(moveDirection.x, 0, moveDirection.z).normalized * moveSpeed;
            rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
        }
    }
}
