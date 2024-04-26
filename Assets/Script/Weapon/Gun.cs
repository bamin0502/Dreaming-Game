using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;
using Random = UnityEngine.Random;
using EPOOutline;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public LineRenderer lineRenderer;
    public float bulletLifeTime = 2f;
    public ParticleSystem fireEffect;
    private Camera mainCamera;
    private int damage;
    public Outlinable outlinable;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Fire();
            fireEffect.Play();
        }
    }

    private void Fire()
    {
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        var targetPoint = ray.GetPoint(100); // 기본 거리 설정
        if (Physics.Raycast(ray, out var hit))
        {
            targetPoint = hit.point;
            ApplyDamage(hit);
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
    private void ApplyDamage(RaycastHit hit)
    {
        damage=Random.Range(10,20);
        if (hit.collider.CompareTag("Enemy"))
        {
            hit.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }
}