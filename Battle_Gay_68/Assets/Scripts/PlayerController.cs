using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private UnityEngine.Vector2 movement;
    private Rigidbody2D rb;

    private void Awake() {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void Update() {
        PlayerInput();
    }
    private void FixedUpdate() {
        Move();
    }
    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<UnityEngine.Vector2>();
    }
    private void Move() {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }
}
