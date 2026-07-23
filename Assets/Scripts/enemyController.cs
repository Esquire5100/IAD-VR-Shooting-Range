using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
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

    public float footstepVolume = 0.7f;
    public AudioClip[] footsteps;

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

        StartCoroutine(PlayFootstepSounds());
    }

    private void Update()
    {
        TargetPlayer();
    }

    public IEnumerator PlayFootstepSounds()
    {
        // Play random footstep sounds only when the agent is moving and not hurt/dead
        if (agent.velocity.magnitude > 0.1f && !isHurt && !isDead)
        {
            int randomIndex = UnityEngine.Random.Range(0, footsteps.Length); 
            AudioClip footstepSound = footsteps[randomIndex];
            AudioSource.PlayClipAtPoint(footstepSound, transform.position, footstepVolume); 
            float delay = footstepSound.length + 0.3f; // Delay based on the length of the footstep sound
            yield return new WaitForSeconds(delay);
        }

        yield return null; // Wait for the next frame before checking again
        StartCoroutine(PlayFootstepSounds());
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
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isAttacking", true);
            Debug.Log("Enemy is attacking");
            StartCoroutine(AttackNEnd());
        }
    }

    public IEnumerator AttackNEnd()
    {
        Debug.Log("Enemy has attacked the player");
        yield return new WaitForSeconds(0.8f);
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
