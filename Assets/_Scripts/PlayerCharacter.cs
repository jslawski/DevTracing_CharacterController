using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    public PlayerControls playerControls;

    [SerializeField]
    private float _moveSpeed = 4.5f;

    private Transform _playerTransform;

    private Vector3 _moveVector;

    public PlayerState currentState;

    private void Awake()
    {
        this._playerTransform = GetComponent<Transform>();
        
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
    }

    private void FixedUpdate()
    {
        this.currentState.FixedUpdateState();

        this._playerTransform.Translate(this._moveVector);
    }

    public void UpdatePlayerMoveVector(Vector3 moveDirection)
    {
        this._moveVector = moveDirection * this._moveSpeed * Time.fixedDeltaTime;
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
}

