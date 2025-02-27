using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    public int currentHealth;
    private Knockback knockback;
    private Flash flash;

    private MonsterController monsterController;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        monsterController = GetComponent<MonsterController>();
        currentHealth = startingHealth;
        if (!PlayerPrefs.HasKey("isWin_" + monsterController.monsterID))
        {
            PlayerPrefs.SetInt("isWin_" + monsterController.monsterID, 0);
            PlayerPrefs.Save();
        }
    }


    private void Update()
    {
        if (currentHealth <= 0)
        {
            TeleportToPuzzel();
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        knockback.GetKnockedBack(PlayerController.Instance.transform, 15f);
        MonsterController monsterController = GetComponent<MonsterController>();
        StartCoroutine(flash.FlashRoutine());

        if (monsterController != null)
        {
            Debug.Log("Puzzle ID: " + monsterController.monsterID);
            Debug.Log("Monster" + monsterController.monsterID + "Status Saved: " + PlayerPrefs.GetInt("isWin_" + monsterController.monsterID));
        }
        else
        {
            Debug.Log("I don't see it");
        }

    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


    private void TeleportToPuzzel()
    {
        if (monsterController == null)
        {
            Debug.LogError("MonsterController not found on this enemy!");
            return;
        }

        // Save the game state before teleporting
        PlayerPrefs.Save();

        // เปลี่ยนซีนไปยังปริศนา
        SceneManager.LoadScene("monster" + monsterController.monsterID);
    }

}
