using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Slime : Enemy
{
    [Header("Slime Settings")]
    [SerializeField] private float DelayToMove;

    private float _timeToNextAttack;
    private float _timeToMove;

    public override void Start()
    {
        base.Start();
        _timeToMove = Time.time + DelayToMove;
    }

    private void Update()
    {
        if (!_isAlive) return;
        if (Time.time < _timeToMove) return;

        if (transform.position.x > _player.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (Vector2.Distance(transform.position, _player.position) <= DistanceToAttack)
        {
            if (Time.time > _timeToNextAttack)
            {
                StartCoroutine(Attack());
                _timeToNextAttack = Time.time + TimeBetweenAttacks;
            }
        } else
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.position, Speed * Time.deltaTime);
        }
    }

    IEnumerator Attack()
    {
        _player.GetComponent<Player>().TakeDamage(Damage);

        Vector2 originalPosition = transform.position;
        Vector2 targetPosition = _player.position;

        float percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime * AttackSpeed;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector2.Lerp(originalPosition, targetPosition, formula);

            yield return null;
        }
    }
}
