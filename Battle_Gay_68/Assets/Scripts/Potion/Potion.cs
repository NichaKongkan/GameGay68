using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Potion : MonoBehaviour
{
    public PotionType potionType;

    public int xIndex;
    public int yIndex;
    public bool isMatched;

    private UnityEngine.Vector2 currentPos;
    private UnityEngine.Vector2 targetPos;

    public Potion(int _x, int _y) {
        xIndex = _x;
        yIndex = _y;
    }

    public void SetIndicies(int _x, int _y) {
        xIndex = _x;
        yIndex = _y;
    }

    public enum PotionType {
        Red,
        Blue,
        Purple,
        Green,
        White,
    }
}
