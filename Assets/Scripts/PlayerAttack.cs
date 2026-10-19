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

    [System.Serializable]
    public class WeaponData
    {
        public WeaponType type;
        public float range;
        public float damage;
    }

    [Header("Weapon is in inventory")]
    [SerializeField]
    private WeaponData[] weapons =
    {
        new WeaponData { type = WeaponType.Knife, range = 1.5f, damage = 25f },
        new WeaponData { type = WeaponType.Pistol, range = 20f, damage = 15f },
        new WeaponData { type = WeaponType.SniperRifle, range = 100f, damage = 80f },
        new WeaponData { type = WeaponType.AssaultRifle, range = 40f, damage = 20f }
    };

    [SerializeField] private LayerMask enemyLayers;

    private int currentWeaponIndex;

    private void Update()
    {
        if (weapons == null || weapons.Length == 0) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
        else if (scroll < 0f)
            currentWeaponIndex = (currentWeaponIndex - 1 + weapons.Length) % weapons.Length;

        if (Input.GetMouseButtonDown(0))
            Attack();
    }

    private void Attack()
    {
        if (weapons == null || weapons.Length == 0) return;

        WeaponData weapon = weapons[currentWeaponIndex];

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            weapon.range,
            enemyLayers
        );

        EnemyHealth closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy == null) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
            closestEnemy.TakeDamage(weapon.damage);
    }
}