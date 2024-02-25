using Godot;

public partial class InteractionArea : Area3D
{
    #region SIGNALS
    [Signal] public delegate void PlayerEnteredEventHandler();
    #endregion

    #region EXPORTS
    [Export] public Node3D InteractionNode;
    #endregion


    #region PUBLIC_VARIABLES
    public bool IsPlayerWithinRange;
    #endregion

    #region VIRTUAL_METHODS
	public override void _Ready()
    {
        GlobalDebug.Assert(InteractionNode is Interactable, $"Error: Node {InteractionNode.Name} does not implement Interactable.");
    }
    #endregion

    #region PUBLIC_METHODS
    public void OnBodyEntered(Node3D body)
    {
        if (body is Player)
        {
            IsPlayerWithinRange = true;
            EmitSignal(SignalName.PlayerEntered);
        }
    }

    public void OnBodyExited(Node3D body)
    {
        if (body is Player)
            IsPlayerWithinRange = false;
    }
    #endregion
}