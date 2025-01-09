using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTopuzzles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<EnemyAi>()) {
            Debug.Log("AAAA");
        }
    }
}
