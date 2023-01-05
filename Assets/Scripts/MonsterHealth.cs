using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    public int currentHealth;
    private bool invincible;
    public GameObject[] dropTemplate;
    public Transform dropSpawnPoint;
    
    void Start()
    {
        currentHealth = maxHealth;
        invincible = false;
    }

    public void MonsterTakeDamage(int damage)
    {
        if (!invincible)
            currentHealth -= damage;

        invincible = true;

        if(invincible)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(FadeTime());
            StartCoroutine(InvincibleTime());
        }

        if(currentHealth <= 0)
        {
            Die();
            DropItem();
        }
    }

    private IEnumerator FadeTime()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    private IEnumerator InvincibleTime()
    {
        yield return new WaitForSeconds(0.1f);
        invincible = false;
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    void DropItem()
    {
        int randomDrop = Random.Range(0, 3);

        // 1/3 chance to drop heart
        if (randomDrop == 0)
        {
            Instantiate(dropTemplate[0], dropSpawnPoint.transform.position, Quaternion.identity);
        }
    }
}
