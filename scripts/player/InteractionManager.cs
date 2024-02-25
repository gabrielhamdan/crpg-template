using Godot;
using Godot.Collections;

public partial class InteractionManager : Node
{
    #region EXPORTS
    [Export] NavigationManager _navigationManager;
    [Export] Character _character;
    #endregion

    #region PRIVATE_VARIABLES
    private Interactable _currentInteractable;
    private bool _isInteracting;
    private GlobalDebug _globalDebug;
    #endregion


    #region VIRTUAL_METHODS
    public override void _Ready()
    {
        base._Ready();

        _globalDebug = GetNode("/root/Debug") as GlobalDebug;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_isInteracting)
            _globalDebug.DebugPanel.AddDebugProperty("Interacting with node", _currentInteractable.ToString());
        else
            _globalDebug.DebugPanel.AddDebugProperty("Interacting with node", "<none>");
    }
    #endregion

    #region PRIVATE_METHODS
    internal void SetInteractableTarget(Dictionary target)
    {
        if(!target.ContainsKey("position") || !target.ContainsKey("collider")) return;

        CollisionObject3D collider = (CollisionObject3D)target["collider"];
        InteractionArea interactionArea;
        
        if (collider is InteractionArea)
        {
            interactionArea = (InteractionArea)collider;
            _currentInteractable = (Interactable)interactionArea.InteractionNode;

            if (interactionArea.IsPlayerWithinRange)
                BeginInteraction(interactionArea);
            else
            {
                _navigationManager.MoveTowardsInteractionArea(interactionArea);
                interactionArea.PlayerEntered += BeginInteraction;
            }
        }
    }

    internal void EndInteraction()
    {
        if (_currentInteractable is null) return;
        _currentInteractable.InteractionArea.PlayerEntered -= BeginInteraction;
        _currentInteractable = null;
        _isInteracting = false;
    }

    private void BeginInteraction()
    {
        BeginInteraction(null);
    }

    private async void BeginInteraction(InteractionArea interactionArea)
    {
        _isInteracting = true;
        _character.InteractionTarget = (Node3D)_currentInteractable;
        _character.IsInteracting = true;
        _navigationManager.TargetPosition = _character.GlobalPosition;

        // TODO
        // interaction specificities
        _currentInteractable.PrintInteractionIndex();
        await ToSignal(GetTree().CreateTimer(1.5), "timeout");

        EndInteraction();
        _character.IsInteracting = false;
    }
    #endregion
}