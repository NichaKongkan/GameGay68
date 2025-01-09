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

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        DetectDeath();
    }

    private void DetectDeath() {
        if (currentHealth <= 0) {
            SceneManager.LoadScene(1);
        }
    }

    
    
}
