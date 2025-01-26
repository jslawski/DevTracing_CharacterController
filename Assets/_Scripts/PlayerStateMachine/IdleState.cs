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
    }

    public override void Exit() 
    {
        base.Exit();

        character.playerControls.PlayerMap.MoveLeft.performed -= this.ChangeToMoveLeft;
        character.playerControls.PlayerMap.MoveRight.performed -= this.ChangeToMoveRight;
    }

    private void ChangeToMoveLeft(InputAction.CallbackContext context)
    {
        this.character.ChangeState(new RunState(MoveDirection.Left));
    }

    private void ChangeToMoveRight(InputAction.CallbackContext context)
    {
        this.character.ChangeState(new RunState(MoveDirection.Right));
    }
}
