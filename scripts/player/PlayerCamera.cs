using Godot;
using Godot.Collections;

public partial class PlayerCamera : Camera3D
{
    #region CONSTANT_VARIABLES
    private readonly int RAY_LEN = 1000;
    #endregion

    #region EXPORTS
    [Export] private Player _player;
    [Export] private Node3D _followTarget;

    [ExportGroup("Camera Specs")]
    [Export] private float FOLLOW_ALPHA = 0.1f;
    [Export] private float MAX_ZOOM_IN = 6f;
    [Export] private float MAX_ZOOM_OUT = 12f;
    [Export] private float ZOOM_SPEED = 0.5f;
    [Export] private float ZOOM_ALPHA = 0.05f;
    #endregion

    #region PRIVATE_VARIABLES
    private float _current_zoom = 7.0f;
    #endregion


    #region VIRTUAL_METHODS
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        FollowPlayer();
        ZoomInOut();
    }
    #endregion

    #region PUBLIC_METHODS
    public Dictionary GetRayQueryResult()
    {
        Godot.Vector2 mousePos = GetViewport().GetMousePosition();
        Godot.Vector3 from = ProjectRayOrigin(mousePos);
        Godot.Vector3 to = from + ProjectRayNormal(mousePos) * RAY_LEN;
        PhysicsDirectSpaceState3D space = GetWorld3D().DirectSpaceState;
        PhysicsRayQueryParameters3D rayQuery = new()
        {
            From = from,
            To = to,
            CollideWithAreas = true
        };

        return space.IntersectRay(rayQuery);
    }
    #endregion

    #region PRIVATE_METHODS
    private void FollowPlayer()
    {
        GlobalPosition = GlobalPosition.Lerp(_followTarget.GlobalPosition, FOLLOW_ALPHA);
    }

    private void ZoomInOut()
    {
        if(Size == _current_zoom) return;

        Size = Mathf.Lerp(Size, _current_zoom, ZOOM_ALPHA);
    }

    internal void ZoomCamera(float zoomValue)
    {
        _current_zoom = Mathf.Clamp(_current_zoom + ZOOM_SPEED * zoomValue, MAX_ZOOM_IN, MAX_ZOOM_OUT);
    }
    #endregion
}
