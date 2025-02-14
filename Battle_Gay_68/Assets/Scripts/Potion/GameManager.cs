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

    public int goal;
    public int moves;
    public int points;

    public bool isGameEnded;
    public bool isWin = false;

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
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space bar");
            WinGame();
        }

        if (isWin) {
            WinGame();
        }
    }

    public void ProcessTurn(int _pointsToGain, bool _subtractMoves) {
        points += _pointsToGain;
        if (_subtractMoves)
            moves--;
        if (points >= goal) {
            //you've won the game
            isGameEnded = true;
            //Display a victory screen
            backgroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);
            isWin = true;
            return;
        
        }
        if (moves == 0) {
            //lose the game
            isGameEnded = true;
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
