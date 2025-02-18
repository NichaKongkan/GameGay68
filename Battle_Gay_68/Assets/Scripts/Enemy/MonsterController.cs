using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public string puzzleID;

    private void Start()
    {
        /**
        int puzzleStatus = PlayerPrefs.GetInt(puzzleID, 0);
        Debug.Log($"[MonsterController] {puzzleID} status: {puzzleStatus}");

        if (puzzleStatus == 1)
        {
            gameObject.SetActive(false);
            Debug.Log($"[MonsterController] {puzzleID} Monster hidden");
        }
        else
        {
            Debug.Log($"[MonsterController] {puzzleID} Monster visible");
        }
        */

        gameObject.SetActive(false);
    }
}
