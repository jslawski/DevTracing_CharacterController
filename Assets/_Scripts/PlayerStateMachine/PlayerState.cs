using UnityEngine;

public class PlayerState
{
    protected PlayerCharacter character;

    public virtual void Enter(PlayerCharacter character) 
    {
        this.character = character;
    }

    public virtual void Exit() { }

    public virtual void UpdateState() { }

    public virtual void FixedUpdateState() { }

    ~PlayerState()
    {
        this.character = null;
    }
}
