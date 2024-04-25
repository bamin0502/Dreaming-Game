using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public LineRenderer lineRenderer;
    public float bulletLifeTime = 2f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Fire();
        }
    }

    private void Fire()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        Vector3 targetPoint = ray.GetPoint(100); // 기본 거리 설정
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }

        ShootEffect(targetPoint).Subscribe(_ => { }, () =>
        {
            lineRenderer.enabled = false; 
        });
    }

    private IObservable<long> ShootEffect(Vector3 target)
    {
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target);
        lineRenderer.enabled = true;

        return Observable.Timer(TimeSpan.FromSeconds(bulletLifeTime));
    }
}