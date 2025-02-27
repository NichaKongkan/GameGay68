using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Generated.PropertyProviders;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string puzzleID;
    public string sceneToGo;
    public GameObject backgroundPanel;
    public GameObject victoryPanel;
    public GameObject losePanel;

    public int goal, moves, points;

    public TMP_Text pointsTxt;
    public TMP_Text movesTxt;
    public TMP_Text goalTxt;

    public static string[] allMonsterIDs = { "A", "B", "C" };

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(int _moves, int _goal)
    {
        moves = _moves;
        goal = _goal;
    }

    // Update is called once per frame
    void Update()
    {
        pointsTxt.text = "Potions: " + points.ToString();
        movesTxt.text = "Moves: " + moves.ToString();
        goalTxt.text = "Goal: " + goal.ToString();

        if ((PlayerPrefs.GetInt("isWin_" + puzzleID) == 1 || moves == 0) && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneToGo);   // 🔹 ถ้าไม่ใช่ Boss → กลับไป Sample1

        }
        CheckAllMonstersIsDead();
    }

    public void ProcessTurn(int _pointsToGain, bool _subtractMoves)
    {

        points += _pointsToGain;
        if (_subtractMoves)
            moves--;


        if (points >= goal)
        {
            //win the game
            PlayerPrefs.SetInt("isWin_" + puzzleID, 1); // Save win status
            PlayerPrefs.Save();
            Debug.Log("Win" + puzzleID + "Status Saved: " + PlayerPrefs.GetInt("isWin_" + puzzleID));

            backgroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);
            return;

        }
        if (moves == 0)
        {
            //lose the game
            PlayerPrefs.SetInt("isWin_" + puzzleID, 0); // Save lose status
            PlayerPrefs.Save();
            Debug.Log("Lose Status Saved: " + PlayerPrefs.GetInt("isWin_" + puzzleID));

            backgroundPanel.SetActive(true);
            losePanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);

            if (puzzleID == "Boss_1")
            {
                ResetMonsterStatus(); // 🔹 รีเซ็ตมอนสเตอร์ให้กลับมาเกิดใหม่
                SceneManager.LoadScene("Sample1"); // 🔹 ถ้าแพ้ Boss → โหลดไป Sample1
            }
            return;
        }
    }

    private void CheckAllMonstersIsDead()
    {
        foreach (string id in allMonsterIDs)
        {
            if ((PlayerPrefs.GetInt("isWin_" + id)) != 1)
            {
                return;
            }
        }

        Debug.Log("All monsers are dead, go to another world");
        List<string> monsterList = new List<string>(allMonsterIDs); // Convert array to List
        monsterList.Add("D"); // Append "D"
        allMonsterIDs = monsterList.ToArray(); // Convert back to array
        Debug.Log(string.Join(", ", allMonsterIDs));
        SceneManager.LoadScene("BeforeBoss1");                  //<----- Load Scene to another world

    }

    private void ResetMonsterStatus()
    {
        foreach (string id in allMonsterIDs)
        {
            PlayerPrefs.SetInt("isWin_" + id, 0); // 🔹 รีเซ็ตให้มอนทุกตัวเกิดใหม่
        }
        PlayerPrefs.Save();
        Debug.Log("All monsters have been reset!");
    }

}
