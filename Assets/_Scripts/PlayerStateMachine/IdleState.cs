using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : PlayerState
{
    public override void Enter(PlayerCharacter character) 
    {
        base.Enter(character);

        this.character = character;

        this.character.playerControls.PlayerMap.MoveLeft.performed += this.ChangeToMoveLeft;
        this.character.playerControls.PlayerMap.MoveRight.performed += this.ChangeToMoveRight;
        
        this.character.UpdatePlayerMoveVector(Vector3.zero);

        if (this.character.playerControls.PlayerMap.MoveLeft.inProgress == true)
        {
            this.character.ChangeState(new RunState(Vector3.left));
        }
        else if (this.character.playerControls.PlayerMap.MoveRight.inProgress == true)
        {
            this.character.ChangeState(new RunState(Vector3.right));
        }
    }

    public override void Exit() 
    {
        base.Exit();

        character.playerControls.PlayerMap.MoveLeft.performed -= this.ChangeToMoveLeft;
        character.playerControls.PlayerMap.MoveRight.performed -= this.ChangeToMoveRight;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        this.character.UpdatePlayerMoveVector(Vector3.zero);

        if (this.character.isGrounded == false)
        {
            this.character.ChangeState(new FallingState(Vector3.zero));
        }
    }

    private void ChangeToMoveLeft(InputAction.CallbackContext context)
    {
        this.character.ChangeState(new RunState(Vector3.left));
    }

    private void ChangeToMoveRight(InputAction.CallbackContext context)
    {
        this.character.ChangeState(new RunState(Vector3.right));
    }
}
