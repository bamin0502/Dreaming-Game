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
    
    [Tooltip("플레이어 캐릭터의 초기화 목록들")]
    private Rigidbody rb;
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    private GameObject player;
    
    [Tooltip("애니메이터의 파라미터 지정")]
    private static readonly int Move = Animator.StringToHash("IsMove");

    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        player = GetComponent<GameObject>();
    }
    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        moveDirection= new Vector3(input.x, 0, input.y);
    }
    public void OnAttack(InputAction.CallbackContext value)
    {
        
    }
    
    void Update()
    {
        
        
    }

    private void RotateTowardsCursor()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Vector3 targetPoint;

        // 레이캐스트가 어떤 대상에 맞거나, 씬의 경계 밖으로 갈 경우를 처리
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            targetPoint = hit.point;
        }
        else
        {
            // 적중하지 않을 경우, ray의 방향으로 멀리 있는 점을 대상으로 사용
            targetPoint = ray.origin + ray.direction * 1000f;  // 예를 들어 1000 단위만큼 먼 점을 설정
        }

        targetPoint.y = transform.position.y; // 플레이어의 높이 유지
        Vector3 directionToLook = targetPoint - transform.position;

        if (directionToLook != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
        }
    }

    private void LateUpdate()
    {
        RotateTowardsCursor();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        var transform1 = _camera.transform;
        var forward = transform1.forward;
        var right = transform1.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 targetDirection = moveDirection.x * right + moveDirection.z * forward;

        if (targetDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + targetDirection * (moveSpeed * Time.deltaTime));
            animator.SetBool(Move, true);
        }
        else
        {
            animator.SetBool(Move, false);
        }
    }
}
