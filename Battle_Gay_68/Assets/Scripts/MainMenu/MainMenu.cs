using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        ResetMonsterStatus();
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ResetMonsterStatus()
    {
        string[] allMonsterIDs = { "A", "B", "C", "D", "E", "F", "G", "H", "I" }; // รหัสของมอนสเตอร์ทั้งหมด
        string[] allBossIDs = { "Boss_1", "Boss_2", "Boss_3" };

        foreach (string id in allMonsterIDs)
        {
            PlayerPrefs.SetInt("isWin_" + id, 0);

        }

        foreach (string boss in allBossIDs)
        {
            PlayerPrefs.SetInt("Relation_" + boss, 0);
        }

        PlayerPrefs.SetInt("RelationshipWithBoss", 0);

        PlayerPrefs.Save();
        Debug.Log("Monsters have been reset!");
        Debug.Log("RelationshipWithBoss has been reset to 0!");
    }
}
