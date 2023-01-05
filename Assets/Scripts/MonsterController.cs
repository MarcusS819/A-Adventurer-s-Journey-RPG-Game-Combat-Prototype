using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class MonsterController : MonoBehaviour
{
    public LayerMask playerLayers;

    public float monsterAttackRate = 1f;
    private float nextMonsterAttackTime = 0f;
    
    public int damage = 1;
    [SerializeField] private float attackRange = 0.5f;
    public Transform attackPoint;

    [SerializeField] private int maxRange = 10;
    public Transform player;
    
    public AIPath current;

    void Update() 
    {
        if(Time.time >= nextMonsterAttackTime)
        {
            Attack();
            nextMonsterAttackTime = Time.time + 1f / monsterAttackRate;
        }

        // Enable the AIPath script when player is within range. Disable when out of range.
        if(Vector3.Distance(player.position, transform.position) <= maxRange)
            current.enabled = true;
        else
            current.enabled = false;
    }

    void Attack()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        foreach(Collider2D player in hitPlayers)
        {
            player.GetComponent<PlayerHealth>().PlayerTakeDamage(damage);
        }
    }


    void OnDrawGizmosSelected() 
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
