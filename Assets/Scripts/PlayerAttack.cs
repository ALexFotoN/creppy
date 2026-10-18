using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public enum WeaponType
    {
        Knife,
        Pistol,
        SniperRifle,
        AssaultRifle
    }

    [SerializeField] private WeaponType weapon = WeaponType.Knife;

    [Header("Range")]
    [SerializeField] private float knifeRange = 1.5f;
    [SerializeField] private float pistolRange = 20f;
    [SerializeField] private float sniperRange = 100f;
    [SerializeField] private float assaultRifleRange = 40f;

    [Header("Damage")]
    [SerializeField] private float knifeDamage = 25f;
    [SerializeField] private float pistolDamage = 15f;
    [SerializeField] private float sniperDamage = 80f;
    [SerializeField] private float assaultRifleDamage = 20f;

    [SerializeField] private LayerMask enemyLayers;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Attack();
    }

    private void Attack()
    {
        float range;
        float damage;

        switch (weapon)
        {
            case WeaponType.Knife:
                range = knifeRange;
                damage = knifeDamage;
                break;

            case WeaponType.Pistol:
                range = pistolRange;
                damage = pistolDamage;
                break;

            case WeaponType.SniperRifle:
                range = sniperRange;
                damage = sniperDamage;
                break;

            case WeaponType.AssaultRifle:
                range = assaultRifleRange;
                damage = assaultRifleDamage;
                break;

            default:
                return;
        }

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            range,
            enemyLayers
        );

        EnemyHealth closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy == null) continue;

            float distance = Vector3.Distance(
                transform.position,
                enemy.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
            closestEnemy.TakeDamage(damage);
    }
}