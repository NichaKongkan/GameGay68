using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 2;
    public int currentHealth;

    private void Start() {
        currentHealth = startingHealth;
        
    }

    private void Update() {
        if (currentHealth <= 0)
        {
            TeleportToPuzzel();
        }
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        MonsterController monsterController = GetComponent<MonsterController>();

        if (monsterController != null) {
            Debug.Log("Puzzle ID: " + monsterController.monsterID);
        } else {
            Debug.Log("I don't see it");
        }
        
        //DetectDeath();
    }

    public void DetectDeath() {
        Destroy(gameObject);
    }

    private void TeleportToPuzzel() {
        MonsterController monsterController = GetComponent<MonsterController>();

        PlayerPrefs.Save();
        SceneManager.LoadScene("monster" + monsterController.monsterID);
    }


    
    
}
