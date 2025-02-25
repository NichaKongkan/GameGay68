using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        ResetMonsterStatus();
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame() {
        Application.Quit();
    }

    private void ResetMonsterStatus() {
        string[] allMonsterIDs = { "A", "B", "C" }; // รหัสของมอนสเตอร์ทั้งหมด
        foreach (string id in allMonsterIDs) {
            PlayerPrefs.SetInt("isWin_" + id, 0);
        }
        PlayerPrefs.Save();
        Debug.Log("Monsters have been reset!");
    }
}
