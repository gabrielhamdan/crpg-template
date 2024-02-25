using Godot;
using Godot.Collections;

public partial class StateMachine : Node
{
    #region EXPORTS
    [Export] private State _currentState;
    #endregion

    #region PRIVATE_VARIABLES
    private Dictionary _states;
    private GlobalDebug _globalDebug;
    #endregion


    #region VIRTUAL_METHODS
    public override void _Ready()
    {
        base._Ready();

        _states = new Dictionary();

        foreach(State child in GetChildren()) {
            _states.Add(child.Name, child);
            child.StateTransition += OnStateTransition;
        }

        _currentState.EnterState();
        _globalDebug = GetNode("/root/Debug") as GlobalDebug;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        _currentState.UpdateState(delta);
        _globalDebug.DebugPanel.AddDebugProperty("Current State", _currentState.Name);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        _currentState.StatePhysicsUpdate(delta);
    }
    #endregion

    #region PUBLIC_METHODS
    public void OnStateTransition(StringName newStateName)
    {
        State newState = (State)_states[newStateName];
        if(newState != null && newState != _currentState) {
            _currentState.ExitState();
            newState.EnterState();
            _currentState = newState;
        } else
            GD.PushWarning(GetPath() + "/" + newStateName + " is not a valid State.");
    }
    #endregion
}