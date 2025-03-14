using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public static int currentWorld = 1;

    public static Dictionary<int, string[]> worldMonsters = new Dictionary<int, string[]>()
    {
        { 1, new string[] { "A", "B", "C" } },
        { 2, new string[] { "D", "E", "F" } },
        { 3, new string[] { "G", "H", "I" } }
    };

    private string[] activeMonsters;

    private void Awake()
    {
        Instance = this;
        activeMonsters = worldMonsters[currentWorld];
    }

    public void Initialize(int _moves, int _goal)
    {
        moves = _moves;
        goal = _goal;
    }

    void Update()
    {
        pointsTxt.text = "Potions: " + points.ToString();
        movesTxt.text = "Moves: " + moves.ToString();
        goalTxt.text = "Goal: " + goal.ToString();

        if ((PlayerPrefs.GetInt("isWin_" + puzzleID) == 1 || moves == 0) && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneToGo);
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
            PlayerPrefs.SetInt("isWin_" + puzzleID, 1);
            PlayerPrefs.Save();

            backgroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);

            // 🔹 หากชนะ Boss ให้ไปฉาก WinBoss
            if (puzzleID == $"Boss_{currentWorld}")
            {
                SceneManager.LoadScene($"WinBoss{currentWorld}");
                currentWorld++;
            }
            return;
        }

        if (moves == 0)
        {
            PlayerPrefs.SetInt("isWin_" + puzzleID, 0);
            PlayerPrefs.Save();

            backgroundPanel.SetActive(true);
            losePanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);

            // 🔹 หากแพ้ Boss ให้กลับไป World ที่เหมาะสม
            if (puzzleID == $"Boss_{currentWorld}")
            {
                if (currentWorld == 1)
                {
                    string[] allMonsterIDs = { "A", "B", "C"};
                    foreach (string id in allMonsterIDs)
                    {
                        PlayerPrefs.SetInt("isWin_" + id, 0);
                    }
                    PlayerPrefs.Save();
                    Debug.Log("Monsters(World1) have been reset!");
                }
                else if (currentWorld == 2)
                {
                    string[] allMonsterIDs = { "D", "E", "F"};
                    foreach (string id in allMonsterIDs)
                    {
                        PlayerPrefs.SetInt("isWin_" + id, 0);
                    }
                    PlayerPrefs.Save();
                    Debug.Log("Monsters(World2) have been reset!");
                }
                SceneManager.LoadScene($"World{currentWorld}");
            }

            return;
        }
    }

    private void CheckAllMonstersIsDead()
    {
        // 🔹 ตรวจสอบว่าฉากปัจจุบันเป็น BeforeBoss หรือ Boss หรือไม่
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene.Contains("BeforeBoss") || currentScene.Contains("Boss"))
        {
            return; // ไม่ต้องเช็กสถานะมอนสเตอร์ในฉากบอส
        }

        foreach (string id in activeMonsters)
        {
            if (PlayerPrefs.GetInt("isWin_" + id) != 1)
            {
                return; // ยังมีมอนสเตอร์ที่ไม่ตาย
            }
        }

        Debug.Log("All monsters are dead in World" + currentWorld);

        if (worldMonsters.ContainsKey(currentWorld))
        {
            SceneManager.LoadScene($"BeforeBoss{currentWorld}");
        }
        else
        {
            Debug.Log("All worlds completed!");
        }
    }


    private void ResetMonsterStatus()
    {
        foreach (string id in worldMonsters[currentWorld])
        {
            PlayerPrefs.SetInt("isWin_" + id, 0);
        }
        PlayerPrefs.Save();
        Debug.Log("All monsters in World " + currentWorld + " have been reset!");
    }
}
