using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public string monsterID;

    private void Start()
    {
        int monsterStatus = PlayerPrefs.GetInt(monsterID, 0);
        Debug.Log($"[MonsterController] {monsterID} status: {monsterStatus}");

        if (monsterStatus == 1)
        {
            gameObject.SetActive(false);
            Debug.Log($"[MonsterController] {monsterID} Monster hidden");
        }
        else
        {
            Debug.Log($"[MonsterController] {monsterID} Monster visible");
        }
        

        //gameObject.SetActive(false);
    }
}
