using UnityEngine;
using UnityEngine.InputSystem;

public class RunState : PlayerState
{
    private Vector3 _currentDirection;

    public RunState(Vector3 newDirection)
    {
        this._currentDirection = newDirection;
    }

    public override void Enter(PlayerCharacter character)
    {
        base.Enter(character);

        if (this._currentDirection == Vector3.left)
        {
            this.character.playerControls.PlayerMap.MoveRight.performed += this.MoveRightPerformed;
            this.character.playerControls.PlayerMap.MoveLeft.canceled += this.MoveLeftOrRightCanceled;
            this.character.UpdatePlayerMoveVector(Vector3.left);
            this.character.spriteRenderer.flipX = true;
        }

        if (this._currentDirection == Vector3.right)
        {
            this.character.playerControls.PlayerMap.MoveLeft.performed += this.MoveLeftPerformed;
            this.character.playerControls.PlayerMap.MoveRight.canceled += this.MoveLeftOrRightCanceled;
            this.character.UpdatePlayerMoveVector(Vector3.right);
            this.character.spriteRenderer.flipX = false;
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (this._currentDirection == Vector3.left)
        {
            this.character.playerControls.PlayerMap.MoveRight.performed -= this.MoveRightPerformed;
            this.character.playerControls.PlayerMap.MoveLeft.canceled -= this.MoveLeftOrRightCanceled;
        }

        if (this._currentDirection == Vector3.right)
        {
            this.character.playerControls.PlayerMap.MoveLeft.performed -= this.MoveLeftPerformed;
            this.character.playerControls.PlayerMap.MoveRight.canceled -= this.MoveLeftOrRightCanceled;
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        this.character.UpdatePlayerMoveVector(this._currentDirection);
    }

    private void MoveLeftPerformed(InputAction.CallbackContext context)
    {
        this.character.ChangeState(new RunState(Vector3.left));
    }

    private void MoveRightPerformed(InputAction.CallbackContext context)
    {
        this.character.ChangeState(new RunState(Vector3.right));
    }

    private void MoveLeftOrRightCanceled(InputAction.CallbackContext context)
    {
        if (this.character.playerControls.PlayerMap.MoveLeft.inProgress == true)
        {
            this.character.ChangeState(new RunState(Vector3.left));
        }
        else if (this.character.playerControls.PlayerMap.MoveRight.inProgress == true)
        {
            this.character.ChangeState(new RunState(Vector3.right));
        }
        else
        {
            this.character.ChangeState(new IdleState());
        }
    }
}
