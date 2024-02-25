using Godot;

public partial class TestInteractableObject : Node3D, Interactable
{
    [Export] public InteractionArea InteractionArea { get; set; }
    [Export] private int _interactionIndex;

    public void PrintInteractionIndex()
    {
        GD.Print($"TestInteractableObject interaction index: {_interactionIndex}.");
    }
}
