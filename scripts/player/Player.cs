using Godot;
using Godot.Collections;

public partial class Player : Character
{
    #region EXPORTS
    [Export] private PlayerCamera _playerCamera;
    [Export] private InteractionManager _interactionManager;
    #endregion


    #region PUBLIC_METHODS
    public void MovePlayer()
    {
        if (IsInteracting) return;
        CancelInteraction();
        Dictionary result = _playerCamera.GetRayQueryResult();
        _navigationManager.SetNavigationTarget(result);
    }

    public void ZoomCamera(float zoomValue)
    {
        _playerCamera.ZoomCamera(zoomValue);
    }

    public void Interact()
    {
        if (IsInteracting) return;
        CancelInteraction();
        Dictionary result = _playerCamera.GetRayQueryResult();
        _interactionManager.SetInteractableTarget(result);
    }
    #endregion

    #region PRIVATE_METHODS
    private void CancelInteraction()
    {
        _interactionManager.EndInteraction();
    }
    #endregion
}