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

    private MonsterController monsterController;

    private void Start() {
        monsterController = GetComponent<MonsterController>();
        currentHealth = startingHealth;
        PlayerPrefs.SetInt("isWin_" + monsterController.monsterID, 0);
        PlayerPrefs.Save();
    }


    private void Update() {
        if (currentHealth <= 0) {
            TeleportToPuzzel();
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
