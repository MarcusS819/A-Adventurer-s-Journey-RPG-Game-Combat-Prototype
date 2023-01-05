using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int maxPlayerHealth = 10;
    [SerializeField] private float loadLevelDelay = 0.5f;
    public int currentPlayerHealth;
    private bool invincible;
    public TMP_Text healthDisplay;

    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        invincible = false;
        healthDisplay.text = ("Health: " + currentPlayerHealth);
    }

    public void PlayerTakeDamage(int damage)
    {
        if (!invincible)
        {
            currentPlayerHealth -= damage;
            healthDisplay.text = ("Health: " + currentPlayerHealth);
        }
        
        invincible = true;

        // Fade the player when hit and set player invinciblility to false after
        if(invincible)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(FadeTime());
            StartCoroutine(InvincibleTime());
        }

        if(currentPlayerHealth < 1)
        {
            Invoke("ReloadScene", loadLevelDelay);
        }
    }

    private IEnumerator FadeTime()
    {
        yield return new WaitForSeconds(0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    private IEnumerator InvincibleTime()
    {
        yield return new WaitForSeconds(0.2f);
        invincible = false;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Heart"))
        {
            if (currentPlayerHealth < 10)
            {
                currentPlayerHealth += 1;
                Destroy(other.gameObject);
                healthDisplay.text = ("Health: " + currentPlayerHealth);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

    void ReloadScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}

