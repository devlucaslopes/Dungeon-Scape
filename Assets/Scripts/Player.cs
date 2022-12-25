using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float Speed;
    [SerializeField] private int Health;

    [Header("Attack Settings")]
    [SerializeField] private WeaponPickup InitialWeapon;
    [SerializeField] private Transform WeaponPoint;
    [SerializeField] private Vector3 WeaponPointOffset;
    [SerializeField] private SpriteRenderer WeaponSkin;
    [SerializeField] private LayerMask EnemyLayer;

    [Header("FX")]
    [SerializeField] private GameObject HitEffect;

    private Rigidbody2D rb;
    private Animator anim;

    private bool _lookToRight = true;
    private float _timeBetweenAttacks;
    private float _attackRange;
    private int _damage;
    private float _timeToNextAttack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        WeaponSkin.sprite = InitialWeapon.Skin;
        _timeBetweenAttacks = InitialWeapon.TimeBetweenAttacks;
        _attackRange = InitialWeapon.AttackRange;
        _damage = InitialWeapon.Damage;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _timeToNextAttack)
        {
            _timeToNextAttack = Time.time + _timeBetweenAttacks;
            anim.SetTrigger("attack");
        }

        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = direction * Speed;

        if (direction.x > 0)
        {
            _lookToRight = true;
            transform.eulerAngles = new Vector3(0, 0, 0);
        } else if (direction.x < 0)
        {
            _lookToRight = false;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (direction.x != 0)
        {
            anim.SetBool("isWalking", true);
        } else
        {
            anim.SetBool("isWalking", false);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        Instantiate(HitEffect, transform.position, Quaternion.identity);

        anim.SetTrigger("hit");

        if (Health <= 0)
        {
            Debug.Log("MO-RREU!");
            anim.SetTrigger("die");
            //_isAlive = false;
            //Destroy(gameObject, 1);
        }
    }

    public void Attack()
    {
        Vector3 offset = _lookToRight ? WeaponPointOffset : new Vector3(WeaponPointOffset.x * -1, WeaponPointOffset.y);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(WeaponPoint.position + offset, _attackRange, EnemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(_damage);
        }
    }

    public void ChangeWeapon(WeaponPickup weaponToEquip)
    {
        WeaponSkin.sprite = weaponToEquip.Skin;
        _timeBetweenAttacks = weaponToEquip.TimeBetweenAttacks;
        _attackRange = weaponToEquip.AttackRange;
        _damage = weaponToEquip.Damage;

        Destroy(weaponToEquip.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 offset = _lookToRight ? WeaponPointOffset : new Vector3(WeaponPointOffset.x * -1, WeaponPointOffset.y);
        Gizmos.DrawWireSphere(WeaponPoint.position + offset, _attackRange);
    }
}
