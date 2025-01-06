using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //to detemine whether the space can be filled with christan or not.
    public bool isUsable;

    public GameObject potion;

    public void Initialize(bool _isUsable, GameObject _potions)
    {
        isUsable = _isUsable;
        potion = _potions;
    }
}
