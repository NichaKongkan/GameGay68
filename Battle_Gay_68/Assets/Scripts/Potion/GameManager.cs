using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Generated.PropertyProviders;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

        //if (backgroundPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("[GameManager] Returning to Sample1");
        //    PlayerPrefs.Save();
        //    SceneManager.LoadScene("Sample1");
        //}
    }

    public void ProcessTurn(int _pointsToGain, bool _subtractMoves) {
        //PuzzleManager puzzleManager = new PuzzleManager();

        points += _pointsToGain;
        if (_subtractMoves)
            moves--;
        if (points >= goal) {
            //you've won the game
            //puzzleManager.PuzzleWin();
            //Display a victory screen
            backgroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);
            return;
        
        }
        if (moves == 0) {
            //lose the game
            backgroundPanel.SetActive(true);
            losePanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);
            return;
        }
    }

    public void WinGame() {
        SceneManager.LoadScene(0);
    }

    public void LoseGame() {
        SceneManager.LoadScene(0);
    }
}
