using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 2;
    public int currentHealth;

    private void Start() {
        MonsterController monsterController = GetComponent<MonsterController>();
        currentHealth = startingHealth;
        PlayerPrefs.SetInt("isWin_" + monsterController.monsterID, 0);

        
    }

    public void isAction(bool status)
    {
        gameObject.SetActive(status);
    }

    private void Update() {
        MonsterController monsterController = GetComponent<MonsterController>();
        if (currentHealth <= 0) {
            TeleportToPuzzel();
        }

        if ((PlayerPrefs.GetInt("isWin_" + monsterController.monsterID)) == 1) {
            isAction(false);
            Debug.Log(monsterController.monsterID + " is dead");
        }


    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        MonsterController monsterController = GetComponent<MonsterController>();

        if (monsterController != null) {
            Debug.Log("Puzzle ID: " + monsterController.monsterID);
            Debug.Log("Monster" + monsterController.monsterID + "Status Saved: " + PlayerPrefs.GetInt("isWin_" + monsterController.monsterID));
        } else {
            Debug.Log("I don't see it");
        }
        
    }


    private void TeleportToPuzzel() {
        MonsterController monsterController = GetComponent<MonsterController>();

        PlayerPrefs.Save();
        SceneManager.LoadScene("monster" + monsterController.monsterID);
    }


    
    
}
