using UnityEngine;
using UnityEngine.InputSystem;

public class JumpState : PlayerState
{
    private float _initialJumpHeight = 0.45f;
    private float _jumpVelocity = 4.22f;

    private Vector3 _jumpVelocityVector;

    private float _currentJumpTime = 0.0f;
    private float _maxJumpTime = 0.289f;

    private bool _isHanging = false;

    private int _currentHangingFrames = 0;
    private int _totalHangingFrames = 5;

    public override void Enter(PlayerCharacter character)
    {
        base.Enter(character);

        this.character.playerControls.PlayerMap.Jump.canceled += this.ChangeToFalling;

        this.character.IncrementPlayerRigidbodyPosition(new Vector3(0.0f, this._initialJumpHeight, 0.0f));

        this._jumpVelocityVector = new Vector3(0.0f, this._jumpVelocity, 0.0f);
    }

    public override void Exit()
    {
        base.Exit();

        this.character.playerControls.PlayerMap.Jump.canceled -= this.ChangeToFalling;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        this.character.UpdatePlayerVelocity(this._jumpVelocityVector);

        if (this._currentJumpTime >= this._maxJumpTime)
        {
            this._isHanging = true;    

            if (this._currentHangingFrames >= this._totalHangingFrames)
            {
                this.character.ChangeState(new FallingState(Vector3.zero));
            }
            else
            {                
                this.character.UpdatePlayerVelocity(Vector3.zero);
            }
        }

        this._currentJumpTime += Time.deltaTime;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (this._isHanging == true)
        {
            this._currentHangingFrames++;
        }
    }

    private void ChangeToFalling(InputAction.CallbackContext context)
    {
        this.character.ChangeState(new FallingState(Vector3.zero));
    }
}
