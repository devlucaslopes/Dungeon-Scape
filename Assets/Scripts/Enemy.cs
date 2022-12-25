using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] protected float Speed;
    [SerializeField] protected int Health;
    [SerializeField] protected int Damage;

    [Header("Attack Settings")]
    [SerializeField] protected float AttackSpeed;
    [SerializeField] protected float DistanceToAttack;
    [SerializeField] protected float TimeBetweenAttacks;

    [Header("Drops")]
    [SerializeField] private WeaponPickup[] Items;
    [Range(0, 100)][SerializeField] private float DropRate = 20;

    [Header("FX")]
    [SerializeField] private GameObject HitEffect;

    private Animator anim;

    protected Transform _player;
    protected bool _isAlive = true;

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        Instantiate(HitEffect, transform.position, Quaternion.identity);
        anim.SetTrigger("hit");

        if (Health <= 0)
        {
            _isAlive = false;
            anim.SetTrigger("die");
            DropItem();
            Destroy(gameObject, 1);
        }
    }

    private void DropItem()
    {
        int rate = Random.Range(0, 101);

        if (rate <= DropRate)
        {
            WeaponPickup randomWeapon = Items[Random.Range(0, Items.Length)];
            Instantiate(randomWeapon, transform.position, Quaternion.identity);
        }
    }
}
