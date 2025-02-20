using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterController : MonoBehaviour
{
    public string monsterID;
    public static string[] allMonsterIDs = {"A", "B", "C"};

    private void Start()
    {
        int monsterStatus = PlayerPrefs.GetInt("isWin_" + monsterID);
        Debug.Log($"[MonsterController] {monsterID} status: {monsterStatus}");

        if (monsterStatus == 1)
        {
            gameObject.SetActive(false);
            Debug.Log($"[MonsterController] {monsterID} Monster hidden");
        }
        else
        {
            Debug.Log($"[MonsterController] {monsterID} Monster visible");
        }

        CheckAllMonstersIsDead();

    }

    private void CheckAllMonstersIsDead() {
        foreach (string id in allMonsterIDs) {
            if ((PlayerPrefs.GetInt("isWin_" + id)) != 1) {
                return;
            }
        }

        Debug.Log("All monsers are dead, go to another world");
        SceneManager.LoadScene("Sample2");

    }
}
