using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;
using Random = UnityEngine.Random;
using EPOOutline;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public LineRenderer lineRenderer;
    public float bulletLifeTime = 0.2f;
    public ParticleSystem fireEffect;
    private Camera mainCamera;
    private int damage;
    private IDisposable outlineSubscription;
    private RaycastHit lastHit;
    private PlayerHealth PlayerHealth;
    private List<Outlinable> outlinables = new List<Outlinable>();
    
    void Start()
    {
        mainCamera = Camera.main;
        PlayerHealth = GetComponentInParent<PlayerHealth>();
    }

    private void OnEnable()
    {
        outlineSubscription=this.UpdateAsObservable()
            .Select(_ =>Mouse.current.position.ReadValue())
            .Select(position => mainCamera.ScreenPointToRay(position))
            .Select(ray =>Physics.Raycast(ray,out lastHit) ? lastHit.collider:null)
            .DistinctUntilChanged()
            .Subscribe(HandleOutLine);
    }

    private void OnDisable()
    {
        outlineSubscription?.Dispose();
    }
    
    private void Update()
    {
        
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
        if(PlayerHealth.isDead) return;
        var ray=new Ray(firePoint.position,firePoint.forward);
        var targetPoint=ray.GetPoint(100f);
        if(Physics.Raycast(ray,out var hit))
        {
            targetPoint = hit.point;
            ApplyDamage(hit);
        }
        
        ShootEffect(targetPoint)
            .Subscribe(_ =>
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
    
    private void HandleOutLine(Collider hitCollider)
    {
        //커서가 닿을때는 초록색 아니면 빨강색 테두리
        foreach (var outlinable in outlinables)
        {
            outlinable.OutlineParameters.Color = hitCollider == null ? Color.red : Color.green;
        }
        outlinables.Clear();
        if (hitCollider != null)
        {
            var outlinable = hitCollider.GetComponent<Outlinable>();
            if (outlinable != null)
            {
                outlinable.OutlineParameters.Color = Color.green;
                outlinables.Add(outlinable);
            }
        }
        
        
    }
}