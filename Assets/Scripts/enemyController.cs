using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyController : MonoBehaviour
{
    public float maxHealth = 100f;

    public float currentHealth;

    public Transform target;
    public NavMeshAgent agent;

    protected bool isHurt;
    protected bool isDead;
    public event Action<enemyController> OnDied;

    public EnemySpawner EnemySpawner;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        TargetPlayer();
    }

    void TargetPlayer()
    {
        agent.SetDestination(target.position);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Keeps health between bounds
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            EnemySpawner.GameEnd();
        }

    }

    void Die()
    {
        OnDied?.Invoke(this);
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        Destroy(gameObject);
    }
}
