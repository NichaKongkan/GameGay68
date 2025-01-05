using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    private void Start() { 
        if (transitionName == SceneManagement.Instance.ScenceTransitionName) { 
            PlayerController.Instance.tranform.position = this.transform.position;
        }
    }
}
