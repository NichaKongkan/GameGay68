using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public string[] puzzleIDs;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            Debug.Log("GO to WIN");;
            PuzzleWin();
        }
    }

    public void PuzzleWin()
    {
        foreach (string id in puzzleIDs)
        {
            PlayerPrefs.SetInt(id, 1);
            Debug.Log($"[PuzzleManager] {id} set to 1");
        }
        PlayerPrefs.Save();
        Debug.Log("[PuzzleManager] PlayerPrefs saved");
        SceneManager.LoadScene("Sample1");
    }
}
