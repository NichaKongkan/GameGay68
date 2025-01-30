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

    public bool isMoving;

    public Potion(int _x, int _y) {
        xIndex = _x;
        yIndex = _y;
    }

    public void SetIndicies(int _x, int _y) {
        xIndex = _x;
        yIndex = _y;
    }

    //MoveToTarget
    public void MoveToTarget(UnityEngine.Vector2 _targetPos) {
        StartCoroutine(MoveCoroutine(_targetPos));
    }
    //MoveCoroutine
    private IEnumerator MoveCoroutine(UnityEngine.Vector2 _targetPos) {
        isMoving = true;
        float duration = 0.2f;

        UnityEngine.Vector2 startPosition = transform.position;
        float elaspedTime = 0f;

        while (elaspedTime < duration) {
            float t = elaspedTime / duration;
            transform.position = UnityEngine.Vector2.Lerp(startPosition, _targetPos, t);
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        
        transform.position = _targetPos;
        isMoving = false;
    }
    public enum PotionType {
        Red,
        Blue,
        Purple,
        Green,
        White,
    }
}
