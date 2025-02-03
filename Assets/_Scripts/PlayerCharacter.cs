using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour
{
    private Transform _playerTransform;

    private Collider _playerCollider;

    private Rigidbody _playerRigidbody;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public PlayerControls playerControls;

    public PlayerState currentState;

    public bool isGrounded = false;
    public bool canJump = false;

    public LayerMask collisionLayerMask;

    private void Awake()
    {
        this._playerTransform = GetComponent<Transform>();
        this._playerCollider = GetComponentInChildren<Collider>();
        this._playerRigidbody = GetComponent<Rigidbody>();
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

        this.UpdateIsGrounded();

        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void FixedUpdate()
    {    
        this.currentState.FixedUpdateState();

        //Debug.LogError("State: " + this.currentState.GetType().ToString() + " Velocity: " + this._playerRigidbody.linearVelocity.x);
    }

    public void UpdatePlayerVelocity(Vector3 newVelocity)
    {
        this._playerRigidbody.linearVelocity = newVelocity;
    }

    public void IncrementPlayerRigidbodyPosition(Vector3 vectorIncrement)
    {
        this._playerRigidbody.position += vectorIncrement;
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

    private void UpdateIsGrounded()
    {
        this.isGrounded = false;
    
        Collider[] colliders = Physics.OverlapBox(this._playerCollider.bounds.center, this._playerCollider.bounds.extents * 1.1f, Quaternion.identity, this.collisionLayerMask);
    
        foreach (Collider collider in colliders) 
        {
            if (Mathf.Abs(collider.bounds.max.y - this._playerCollider.bounds.min.y) <= 0.01f)
            {
                this.isGrounded = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {    
        foreach (ContactPoint point in collision.contacts)
        {
            if (point.normal == Vector3.up)
            {
                //Debug.LogError("Adjusting");
                //this._playerRigidbody.position = new Vector3(this._playerRigidbody.position.x, this._playerRigidbody.position.y + point.separation, this._playerRigidbody.position.z);    
                //this.canJump = true;
            }
        }
        
    }

}

