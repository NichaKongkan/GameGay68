using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 2;
    public int currentHealth;
    public static bool PuzzelWin = false;


    private void Start() {
        currentHealth = startingHealth;
    }

    void Update() {
        if ((currentHealth <= 0) && PuzzelWin) {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        TeleportToPuzzel();
        //DetectDeath();
    }

    public void DetectDeath() {
        //if ((currentHealth <= 0) && PuzzelWin) {
        //    Destroy(gameObject);
        //}
        Destroy(gameObject);
    }

    private void TeleportToPuzzel() {
        if (currentHealth <= 0) {
            SceneManager.LoadScene(1);
        }
    }

    public static void SetPuzzelWin(bool winStatus) {
        PuzzelWin = winStatus;
    }

    
    
}
