using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health = 50f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Enemy health: {health}");

        if (health <= 0f)
            Destroy(gameObject);
    }
}