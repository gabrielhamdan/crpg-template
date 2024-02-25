using Godot;

public partial class IdleState : State
{
    #region EXPORTS
    [Export] private  Character _character;
    #endregion


    #region PUBLIC_METHODS
    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState(double delta)
    {
        if(_character.Velocity.Length() > 0.0) {
            EmitSignal("StateTransition", "WalkingState");
            return;
        }

        SetBlendPosition();
    }

        public override void StatePhysicsUpdate(double delta)
    {
        
    }
    #endregion

    #region PRIVATE_METHODS
    private void SetBlendPosition()
    {
        float speed = _character.Speed;
        var alpha = Mathf.Remap(speed, 0.0, _character.MaxSpeed, 0.0, 1.0);
        animationTree.Set("parameters/IWR/blend_position", (float)Mathf.Lerp(speed, 0.0, alpha));
    }
    #endregion
}