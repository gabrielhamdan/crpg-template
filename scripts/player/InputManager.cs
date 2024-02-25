using Godot;

public partial class InputManager : Node
{
    #region CONSTANT_VARIABLES
    private readonly string MOVE_CLICK = "move_click";
    private readonly string ACTION_CLICK = "action_click";
    private readonly string ZOOM_IN = "zoom_in";
    private readonly string ZOOM_OUT = "zoom_out";
    private readonly string TOGGLE_DEBUG = "toggle_debug";
    private readonly string QUIT = "quit";
    #endregion

    #region EXPORTS
    [Export] private Player _player;
    #endregion

    #region PRIVATE_VARIABLES
    private GlobalDebug _globalDebug;
    #endregion


    #region VIRTUAL_METHODS
    public override void _Ready()
    {
        base._Ready();

        _globalDebug = GetNode<GlobalDebug>("/root/Debug");
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (Input.IsActionPressed(MOVE_CLICK, true))
        {
            if (IsDoubleClick(@event))
                _player.IsRunning = true;

            _player.MovePlayer();
        }

        if (Input.IsActionJustPressed(ACTION_CLICK, true))
        {
            if (IsDoubleClick(@event))
                _player.IsRunning = true;

            _player.Interact();
        }

        if (Input.IsActionJustPressed(ZOOM_IN, true))
        {
            var zoomIn = -1f;
            _player.ZoomCamera(zoomIn);
        } else if (Input.IsActionJustPressed(ZOOM_OUT, true))
        {
            var zoomOut = 1f;
            _player.ZoomCamera(zoomOut);
        }

        if (Input.IsActionJustPressed(TOGGLE_DEBUG, true))
            _globalDebug.ToggleDebug();

        if (Input.IsActionJustPressed(QUIT, true))
            GetTree().Quit();
    }
    #endregion

    #region PRIVATE_METHODS
    private bool IsDoubleClick(InputEvent @event)
    {
        InputEventMouseButton inputEventMouseButton = @event as InputEventMouseButton;
        return inputEventMouseButton != null && inputEventMouseButton.DoubleClick;
    }
    #endregion
}