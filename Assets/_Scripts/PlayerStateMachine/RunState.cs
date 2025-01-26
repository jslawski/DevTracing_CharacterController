using UnityEngine;
using UnityEngine.InputSystem;

public enum MoveDirection { Left = -1, Right = 1 };

public class RunState : PlayerState
{
    private MoveDirection _currentDirection;

    public RunState(MoveDirection newDirection)
    {
        this._currentDirection = newDirection;
    }

    public override void Enter(PlayerCharacter character)
    {
        base.Enter(character);

        if (this._currentDirection == MoveDirection.Left)
        {
            this.character.playerControls.PlayerMap.MoveRight.performed += this.MoveRightPerformed;
            this.character.playerControls.PlayerMap.MoveLeft.canceled += this.MoveLeftOrRightCanceled;
            this.character.UpdatePlayerMoveVector(Vector3.left);
        }

        if (this._currentDirection == MoveDirection.Right)
        {
            this.character.playerControls.PlayerMap.MoveLeft.performed += this.MoveLeftPerformed;
            this.character.playerControls.PlayerMap.MoveRight.canceled += this.MoveLeftOrRightCanceled;
            this.character.UpdatePlayerMoveVector(Vector3.right);
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (this._currentDirection == MoveDirection.Left)
        {
            this.character.playerControls.PlayerMap.MoveRight.performed -= this.MoveRightPerformed;
            this.character.playerControls.PlayerMap.MoveLeft.canceled -= this.MoveLeftOrRightCanceled;
        }

        if (this._currentDirection == MoveDirection.Right)
        {
            this.character.playerControls.PlayerMap.MoveLeft.performed -= this.MoveLeftPerformed;
            this.character.playerControls.PlayerMap.MoveRight.canceled -= this.MoveLeftOrRightCanceled;
        }
    }

    private void MoveLeftPerformed(InputAction.CallbackContext context)
    {
        this.character.ChangeState(new RunState(MoveDirection.Left));
    }

    private void MoveRightPerformed(InputAction.CallbackContext context)
    {
        this.character.ChangeState(new RunState(MoveDirection.Right));
    }

    private void MoveLeftOrRightCanceled(InputAction.CallbackContext context)
    {
        if (this.character.playerControls.PlayerMap.MoveLeft.inProgress == true)
        {
            this.character.ChangeState(new RunState(MoveDirection.Left));
        }
        else if (this.character.playerControls.PlayerMap.MoveRight.inProgress == true)
        {
            this.character.ChangeState(new RunState(MoveDirection.Right));
        }
        else
        {
            this.character.ChangeState(new IdleState());
        }
    }
}
