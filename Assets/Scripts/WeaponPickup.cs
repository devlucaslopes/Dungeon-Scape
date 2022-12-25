using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] public int Damage;
    [SerializeField] public float TimeBetweenAttacks;
    [SerializeField] public float AttackRange;
    [SerializeField] public Sprite Skin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().ChangeWeapon(this);
        }
    }
}
