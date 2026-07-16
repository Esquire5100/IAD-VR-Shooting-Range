using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class enemyController : MonoBehaviour
{
    public float maxHealth = 100f;

    public float currentHealth;

    public Transform target;
    public NavMeshAgent agent;
    public Animator animator;

    protected bool isHurt;
    protected bool isDead;
    public event Action<enemyController> OnDied;

    public EnemySpawner EnemySpawner;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        EnemySpawner = FindObjectOfType<EnemySpawner>();
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
        animator.SetBool("isAttacking", true);
        Debug.Log("Enemy is attacking");
        StartCoroutine(AttackNEnd());
    }

    public IEnumerator AttackNEnd()
    {
        Debug.Log("Enemy has attacked the player");
        yield return new WaitForSeconds(2f);
        animator.SetBool("isAttacking", false);
        if (SceneManager.GetActiveScene().name == "Simulation")
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
