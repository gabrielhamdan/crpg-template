using Godot;

public abstract partial class State : Node
{
    [Signal] public delegate void StateTransitionEventHandler(StringName newStateName);

    [Export] protected AnimationTree animationTree;

    protected GlobalDebug globalDebug;

    public override void _Ready()
    {
        base._Ready();

        globalDebug = GetNode<GlobalDebug>("/root/Debug");
    }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void UpdateState(double delta);

    public abstract void StatePhysicsUpdate(double delta);
}
