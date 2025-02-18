using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Generated.PropertyProviders;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string puzzleID;
    public GameObject backgroundPanel;
    public GameObject victoryPanel;
    public GameObject losePanel;

    public int goal, moves, points;

    public TMP_Text pointsTxt;
    public TMP_Text movesTxt;
    public TMP_Text goalTxt;

    private void Awake() {
        Instance = this;
    }

    public void Initialize(int _moves, int _goal) {
        moves = _moves;
        goal = _goal;
    }

    // Update is called once per frame
    void Update()
    {
        pointsTxt.text = "Potions: " + points.ToString();
        movesTxt.text = "Moves: " + moves.ToString();
        goalTxt.text = "Goal: " + goal.ToString();

        if ((PlayerPrefs.GetInt("isWin_" + puzzleID) == 1 || moves == 0) && Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene("Sample1");
        }
    }

    public void ProcessTurn(int _pointsToGain, bool _subtractMoves) {

        points += _pointsToGain;
        if (_subtractMoves)
            moves--;


        if (points >= goal) {
            //win the game
            PlayerPrefs.SetInt("isWin_" + puzzleID, 1); // Save win status
            PlayerPrefs.Save();
            Debug.Log("Win" + puzzleID +"Status Saved: " + PlayerPrefs.GetInt("isWin_" + puzzleID));

            backgroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);
            return;
        
        }
        if (moves == 0) {
            //lose the game
            PlayerPrefs.SetInt("isWin_" + puzzleID, 0); // Save lose status
            PlayerPrefs.Save();
            Debug.Log("Lose Status Saved: " + PlayerPrefs.GetInt("isWin_" + puzzleID));
            
            backgroundPanel.SetActive(true);
            losePanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);
            return;
        }
    }

}
