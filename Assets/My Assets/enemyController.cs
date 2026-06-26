using UnityEngine;

public class enemyController : MonoBehaviour
{
    public float health = 100f;

    void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
