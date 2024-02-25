using Godot;

public partial class WalkingState : State
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
        if(_character.Velocity.Length() == 0)
            EmitSignal("StateTransition", "IdleState");

        SetBlendPosition();
    }

    public override void StatePhysicsUpdate(double delta)
    {
        
    }
    #endregion

    #region PRIVATE_METHODS
    private void SetBlendPosition()
    {
        var alpha = Mathf.Remap(_character.Speed, 0.0, _character.MaxSpeed, 0.0, 1.0);
        var blendPosition = (float)Mathf.Clamp(Mathf.Lerp(0.0, 1.0, alpha), 0.0, _character.IsRunning ? 1.0 : 0.5);
        animationTree.Set("parameters/IWR/blend_position", blendPosition);
    }
    #endregion
}