using Godot;

public partial class DebugPanel : PanelContainer
{
    #region EXPORTS
	[Export] private VBoxContainer _propertyContainer;
    [Export] private bool _isDebugMode;
    #endregion

    #region PRIVATE_VARIABLES
    private GlobalDebug _globalDebug;
    private string _fps;
    #endregion


    #region VIRTUAL_METHODS
    public override void _Ready()
    {
        base._Ready();

        _globalDebug = GetNode<GlobalDebug>("/root/Debug");
        _globalDebug.DebugPanel = this;
        _globalDebug.IsDebugMode = _isDebugMode;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        Visible = _globalDebug.IsDebugMode;
        if(Visible)
            UpdateDebugProperties(delta);
    }
    #endregion

    #region PUBLIC_METHODS
    public void AddDebugProperty(string name, string value)
    {
        AddDebugProperty(name, value, 1);
    }

	public void AddDebugProperty(string name, string value, int order)
    {
        Label target = _propertyContainer.FindChild(name, true, false) as Label;

        if(target == null) {
            target = new Label();
            _propertyContainer.AddChild(target);
            target.Name = name;
            target.Text = target.Name + ": " + value;
        } else if(Visible) {
            target.Text = target.Name + ": " + value;
            _propertyContainer.MoveChild(target, order);
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private void UpdateDebugProperties(double delta)
    {
        _fps = (1.0 / delta).ToString("0.00");
        AddDebugProperty("FPS", _fps, 0);
    }
    #endregion
}