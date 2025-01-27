using UnityEngine;

public class FallingState : PlayerState
{
    public override void Enter(PlayerCharacter character)
    {
        base.Enter(character);

        this.character.UpdatePlayerMoveVector(Vector3.zero);
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (this.character.isGrounded == true)
        {
            this.character.ChangeState(new IdleState());
        }
    }

}
