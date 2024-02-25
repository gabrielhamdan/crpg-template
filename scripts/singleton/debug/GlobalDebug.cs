using System;
using Godot;

[GlobalClass]
public partial class GlobalDebug : Node
{
    #region PUBLIC_VARIABLES
    public bool IsDebugMode;
    public DebugPanel DebugPanel;
    #endregion


    #region VIRTUAL_METHODS
    public override void _Ready()
    {
        base._Ready();

        AddToGroup("debug");
    }
    #endregion

    #region PUBLIC_METHODS
    public void ToggleDebug()
    {
        IsDebugMode = !IsDebugMode;
    }
    #endregion

    #region PRIVATE_METHODS
    internal static void Assert(bool cond, string msg)
    {
        if (cond) return;

        GD.PrintErr(msg);
        throw new ApplicationException($"Assert Failed: {msg}");
    }
    #endregion
}
