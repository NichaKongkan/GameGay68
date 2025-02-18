using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public string puzzleID;

    private void Update()
    {
        GameManager gameManager = GetComponent<GameManager>();
        
    }

}

/**
if (Input.GetKeyDown(KeyCode.Space) && gameManager.isWin)
        {   
            Debug.Log("GO to WIN");;
            
        }

*/