using System.Collections.Generic;
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

    [Header("Weapon chars")]
    [SerializeField]
    private WeaponData[] weaponDefinitions =
    {
        new WeaponData { type = WeaponType.Knife, range = 1.5f, damage = 25f },
        new WeaponData { type = WeaponType.Pistol, range = 20f, damage = 15f },
        new WeaponData { type = WeaponType.SniperRifle, range = 100f, damage = 80f },
        new WeaponData { type = WeaponType.AssaultRifle, range = 40f, damage = 20f }
    };

    [SerializeField] private LayerMask enemyLayers;

    private readonly List<WeaponData> inventory = new List<WeaponData>();
    private int currentWeaponIndex;

    private void Update()
    {
        if (inventory.Count == 0) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
            currentWeaponIndex = (currentWeaponIndex + 1) % inventory.Count;
        else if (scroll < 0f)
            currentWeaponIndex = (currentWeaponIndex - 1 + inventory.Count) % inventory.Count;

        if (Input.GetMouseButtonDown(0))
            Attack();
    }

    public bool HasWeapon(WeaponType type)
    {
        return inventory.Exists(weapon => weapon.type == type);
    }

    public bool AddWeapon(WeaponType type)
    {
        if (HasWeapon(type)) return false;

        foreach (WeaponData definition in weaponDefinitions)
        {
            if (definition.type == type)
            {
                inventory.Add(new WeaponData
                {
                    type = definition.type,
                    range = definition.range,
                    damage = definition.damage
                });

                currentWeaponIndex = inventory.Count - 1;
                Debug.Log($"Picked weapon: {type}");
                return true;
            }
        }

        Debug.LogWarning($"Didn't get weapon chars: {type}");
        return false;
    }

    private void Attack()
    {
        WeaponData weapon = inventory[currentWeaponIndex];

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