using Godot;

public partial class CameraTarget : Node3D
{
    #region EXPORTS
    [Export] private Node3D _target;
    #endregion

    #region PRIVATE_VARIABLES
    private Vector3 _offset;
    #endregion


    #region VIRTUAL_METHODS
    public override void _Ready()
    {
        base._Ready();

        _offset = Position;
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        GlobalPosition = _target.GlobalPosition + _offset;
    }
    #endregion
}