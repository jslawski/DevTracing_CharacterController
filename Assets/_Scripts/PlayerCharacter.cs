using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    private float _moveSpeed = 4.5f;
    
    private float _fallSpeed = 8.5f;

    private Transform _playerTransform;

    private Collider _playerCollider;

    private Vector3 _moveVector;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public PlayerControls playerControls;

    public PlayerState currentState;

    public bool isGrounded = false;

    public LayerMask collisionLayerMask;

    private void Awake()
    {
        this._playerTransform = GetComponent<Transform>();
        this._playerCollider = GetComponentInChildren<Collider>();
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        this.playerControls = new PlayerControls();

        this.ChangeState(new IdleState());
    }

    private void OnEnable()
    {
        this.playerControls.Enable();
    }

    private void OnDisable()
    {
        this.playerControls.Disable();
    }

    private void Update()
    {
        this.currentState.UpdateState();

        Debug.LogError("State: " + this.currentState.GetType().ToString());
    }

    private void FixedUpdate()
    {
        this.currentState.FixedUpdateState();

        this.HandleWallCollision();

        this._playerTransform.Translate(this._moveVector);

        this.UpdateIsGrounded();
    }

    public void UpdatePlayerMoveVector(Vector3 moveDirection)
    {
        Vector3 horizontalMovement = moveDirection * this._moveSpeed * Time.fixedDeltaTime;

        Vector3 verticalMovement = Vector3.zero;
        /*
        if (this.isGrounded == false)
        {
            verticalMovement = Vector3.down * this._fallSpeed * Time.fixedDeltaTime;
        }

        
        */

        this._moveVector = horizontalMovement + verticalMovement;
    }

    public void ChangeState(PlayerState newState)
    {
        if (this.currentState != null)
        {
            this.currentState.Exit();
        }
    
        this.currentState = newState;
        this.currentState.Enter(this);
    }

    private void HandleWallCollision()
    {
        RaycastHit hitInfo;
        if (Physics.BoxCast(this._playerTransform.position, this._playerCollider.bounds.extents * 0.9f, this._moveVector.normalized, out hitInfo, Quaternion.identity, this._moveVector.x) == true)
        {
            float xDifference = Mathf.Abs(hitInfo.point.x - this._playerTransform.position.x) - this._playerCollider.bounds.extents.x;

            if (this._moveVector.x > 0.0f)
            {
                this._moveVector = new Vector3(xDifference, this._moveVector.y, this._moveVector.z);
            }
            else if (this._moveVector.x < 0.0f)
            {
                this._moveVector = new Vector3(-xDifference, this._moveVector.y, this._moveVector.z);
            }
        }
    }

    private void UpdateIsGrounded()
    {
        

        

        this.isGrounded = Physics.BoxCast(this._playerTransform.position, this._playerCollider.bounds.extents, Vector3.down, Quaternion.identity, this._playerCollider.bounds.extents.y / 2.0f);
    }
}

