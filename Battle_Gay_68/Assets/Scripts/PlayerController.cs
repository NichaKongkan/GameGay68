using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get {return facingLeft; } set {facingLeft = value; } }
    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private UnityEngine.Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private bool facingLeft = false;

    protected override void Awake() {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void Update() {
        PlayerInput();
    }
    private void FixedUpdate() {
        AdjustPlayerFacingDirection();
        Move();
    }
    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<UnityEngine.Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }
    private void Move() {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection() {
        UnityEngine.Vector3 mousePos = Input.mousePosition;
        UnityEngine.Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x) {
            mySpriteRender.flipX = true;
            FacingLeft = true;
        } else {
            mySpriteRender.flipX = false;
            FacingLeft = false;
        }
    }
}
