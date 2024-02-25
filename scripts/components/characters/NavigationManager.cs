using Godot;
using Godot.Collections;

public partial class NavigationManager : NavigationAgent3D
{
    #region EXPORTS
    [Export] private Character _character;
    #endregion


    #region VIRTUAL_METHODS
    public override void _Ready()
    {
        base._Ready();

        VelocityComputed += OnNavigationAgent3DVelocityComputed;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        MoveToTargetPosition(delta);
    }
    #endregion

    #region PUBLIC_METHODS
    public void SetNavigationPath(Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        if(!IsTargetReachable())
            TargetPosition = GetNextPathPosition();
    }

    public void MoveTowardsInteractionArea(InteractionArea interactionArea)
    {
        Vector3 characterPosition = _character.GlobalPosition;
        CapsuleShape3D characterColliderShape = _character.GetNode<CollisionShape3D>("CollisionShape3D").Shape as CapsuleShape3D;
        float characterColliderRadius = characterColliderShape.Radius / 2.0f;
    
        Transform3D targetColliderTransform = interactionArea.GlobalTransform;
        BoxShape3D targetColliderShape = interactionArea.GetNode<CollisionShape3D>("CollisionShape3D").Shape as BoxShape3D;
        Transform3D invertedTargetColliderTransform = targetColliderTransform.Inverse();
        Vector3 halfTargetColliderExtents = targetColliderShape.Size / 2.0f;

        Vector3 targetPosition = invertedTargetColliderTransform * characterPosition;
        targetPosition.X = Mathf.Clamp(targetPosition.X, -halfTargetColliderExtents.X + characterColliderRadius, halfTargetColliderExtents.X - characterColliderRadius);
        targetPosition.Y = Mathf.Clamp(targetPosition.Y, -halfTargetColliderExtents.Y, halfTargetColliderExtents.Y);
        targetPosition.Z = Mathf.Clamp(targetPosition.Z, -halfTargetColliderExtents.Z + characterColliderRadius, halfTargetColliderExtents.Z - characterColliderRadius);

        var closestTargetPosition = targetColliderTransform.Origin + targetColliderTransform.Basis * targetPosition;

        SetNavigationPath(closestTargetPosition);
    }

    public void OnNavigationAgent3DVelocityComputed(Vector3 safeVelocity)
    {
        _character.Velocity = safeVelocity.Normalized() * _character.Speed;
        _character.MoveAndSlide();
    }
    #endregion

    #region PRIVATE_METHODS
    internal void SetNavigationTarget(Dictionary target)
    {
        if(!target.ContainsKey("position") || !target.ContainsKey("collider")) return;

        CollisionObject3D collider = (CollisionObject3D)target["collider"];
        // if(collider.CollisionLayer == _PATHABLE_LAYER)
        Vector3 targetPosition = (Godot.Vector3)target["position"];
        SetNavigationPath(targetPosition);
    }

    private void MoveToTargetPosition(double delta)
    {
        _character.Accelerate(delta);

        if (IsNavigationFinished() || GetCurrentNavigationPath().Length == 0) return;

        Godot.Vector3 targetPos = GetNextPathPosition();
        Godot.Vector3 direction = _character.GlobalPosition.DirectionTo(targetPos);

        _character.FaceTowards(targetPos, delta);

        Velocity = direction * _character.Speed;
    }
    #endregion
}