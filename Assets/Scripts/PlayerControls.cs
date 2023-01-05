using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControls : MonoBehaviour
{
    private bool isMoving;
    private bool isAttacking;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public LayerMask colliderLayer;
    
    private float nextAttackTime = 0f;
    [SerializeField] private float attackRate = 2.4f;
    [SerializeField] private float lerpDuration = 0.25f;
    [SerializeField] private float attackRange = 0.45f;
    [SerializeField] private int attackDamage = 1;

    Vector3 playerPosition;
    Vector3 targetPosition;

    public Animator anim;

    void Start() 
    {
        isAttacking = anim.GetBool("Attacking");
        playerPosition = transform.position;
    }
    void Update()
    {
        MoveControls();
        AttackControls();
    }

    void MoveControls()
    {

        /* Move keys can only be pressed as long as the player isnt moving
           Set new attack point in correct direction
        */
        if(!isMoving && !isAttacking)
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                if(!anim.GetBool("Move"))
                {
                    StartCoroutine(MovePlayer(Vector3.up));
                    attackPoint.localPosition = new Vector3(0f, 0.5f, 0f);
                    
                }
            }

            if(Input.GetKey(KeyCode.DownArrow))
            {

                if(!anim.GetBool("Move"))
                {
                    StartCoroutine(MovePlayer(Vector3.down));
                    attackPoint.localPosition = new Vector3(0f, -0.5f, 0f);
                }
            }

            if(Input.GetKey(KeyCode.LeftArrow))
            {
                if(!anim.GetBool("Move"))
                {
                    StartCoroutine(MovePlayer(Vector3.left));
                    attackPoint.localPosition = new Vector3(-0.5f, 0f, 0f);
                }
            
            }

            if(Input.GetKey(KeyCode.RightArrow))
            {
                if(!anim.GetBool("Move"))
                {
                    StartCoroutine(MovePlayer(Vector3.right));
                    attackPoint.localPosition = new Vector3(0.5f, 0f, 0f);
                
                }
            }
        }
    }

    void AttackControls()
    {   
        // Attack when current time is greater than next attack time and when not attacking and when not moving
        if(Time.time >= nextAttackTime && !isAttacking && !isMoving)
        {
            if(Input.GetKey(KeyCode.X))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        float idleHorizontal = anim.GetFloat("IdleHorizontal");
        float idleVertical = anim.GetFloat("IdleVertical");

        // Attack when not moving and when not attacking
        if(!isMoving && !isAttacking)
        {
            anim.SetBool("Attacking", true);
            isAttacking = anim.GetBool("Attacking");

            if(idleHorizontal == 1f)
            {
                anim.SetFloat("AttackVertical", 0f);
                anim.SetFloat("AttackHorizontal", 1f);
            }
            else if(idleHorizontal == -1f)
            {
                anim.SetFloat("AttackVertical", 0f);
                anim.SetFloat("AttackHorizontal", -1f);
            }

            if(idleVertical == 1f)
            {
                anim.SetFloat("AttackHorizontal", 0f);
                anim.SetFloat("AttackVertical", 1f);
            }
            else if(idleVertical == -1f || (idleVertical == 0 && idleHorizontal == 0))
            {
                anim.SetFloat("AttackHorizontal", 0f);
                anim.SetFloat("AttackVertical", -1f);
            }

            Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D monster in hitMonsters)
            {
                monster.GetComponent<MonsterHealth>().MonsterTakeDamage(attackDamage);
            }
        }
        
        StartCoroutine(AttackWaitTime());
    }

    private IEnumerator AttackWaitTime()
    {
        yield return new WaitForSeconds(0.35f);
        anim.SetBool("Attacking", false);
        isAttacking = anim.GetBool("Attacking");
    }
    void OnDrawGizmosSelected() 
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    private IEnumerator MovePlayer(Vector3 direction)
    {
        playerPosition = transform.position;
        targetPosition = playerPosition + direction;

        bool onCollider = Physics2D.Raycast(playerPosition, direction, 1f, colliderLayer);

        // Only move on anything that does not have a collider layer
        if(!onCollider)
        {
            SetMoveDirection(direction);
            anim.SetBool("Move", true);

            isMoving = true;

            float elapsedTime = 0;
        
            /* Players position will get closer to target position every iteration
               when elapsed time is less than the lerp duration.

               The third parameter gets closer to 1 every iteration, thus returning the target position.
            */
            while(elapsedTime < lerpDuration)
            {
                transform.position = Vector3.Lerp(playerPosition, targetPosition, (elapsedTime / lerpDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensures that the player is at the target position after the loop
            transform.position = targetPosition;

            isMoving = false;

            anim.SetBool("Move", false);
            anim.SetFloat("Vertical", 0f);
            anim.SetFloat("Horizontal", 0f);

            SetIdleDirection(direction);
        }
        else
        {
            SetIdleDirection(direction);
        }
    }
        
    
    void SetMoveDirection(Vector3 direction)
    {
        if(direction == Vector3.up)
            anim.SetFloat("Vertical", 1f);
        else if(direction == Vector3.down)
            anim.SetFloat("Vertical", -1f);
        else if(direction == Vector3.left)
            anim.SetFloat("Horizontal", -1f);
        else if(direction == Vector3.right)
            anim.SetFloat("Horizontal", 1f);
        
    }
    void SetIdleDirection(Vector3 direction)
    {
        float horizontal = direction.x;
        float vertical = direction.y;

        // Set idle position after player is not moving
        if(horizontal == 1f)
        {
            anim.SetFloat("IdleVertical", 0f);
            anim.SetFloat("IdleHorizontal", 1f);
        }
        else if(horizontal == -1f)
        {
            anim.SetFloat("IdleVertical", 0f);
            anim.SetFloat("IdleHorizontal", -1f);
        }

        if(vertical == 1f)
        {
            anim.SetFloat("IdleHorizontal", 0f);
            anim.SetFloat("IdleVertical", 1f);
        }
        else if(vertical == -1f)
        {
            anim.SetFloat("IdleHorizontal", 0f);
            anim.SetFloat("IdleVertical", -1f);
        }
    }

}
