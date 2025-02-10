using System.Collections;
using System.Collections.Generic;
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

    private void Awake() {
        Instance = this;
    }

    public void Initialize(int _moves, int _goal) {
        moves = _moves;
        goal = _goal;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
        if (moves == 0) {
            //lose the game
            isGameEnded = true;
            backgroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
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
