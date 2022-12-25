using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControls : MonoBehaviour
{
    private bool isMoving;
    [SerializeField] float lerpDuration = 0.3f;
    public Animator anim;

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {

    // Move keys can only be pressed as long as the player isnt moving
    // Start Couroutine and set the animation when the animation boolean to move is false
        if(!isMoving)
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                if(!anim.GetBool("Move"))
                {
                    StartCoroutine(MovePlayer(Vector3.up));
                    anim.SetBool("Move", true);
                    anim.SetFloat("Vertical", 1f);
                }
            }

            if(Input.GetKey(KeyCode.DownArrow))
            {
                if(!anim.GetBool("Move"))
                {
                    StartCoroutine(MovePlayer(Vector3.down));
                    anim.SetBool("Move", true);
                    anim.SetFloat("Vertical", -1f);
                }
            }
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                if(!anim.GetBool("Move"))
                {
                    StartCoroutine(MovePlayer(Vector3.left));
                    anim.SetBool("Move", true);
                    anim.SetFloat("Horizontal", -1f);
                }
            }

            if(Input.GetKey(KeyCode.RightArrow))
            {
                if(!anim.GetBool("Move"))
                {
                    StartCoroutine(MovePlayer(Vector3.right));
                    anim.SetBool("Move", true);
                    anim.SetFloat("Horizontal", 1f);
                }
            }
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        var playerPosition = transform.position;
        var targetPosition = playerPosition + direction;

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
