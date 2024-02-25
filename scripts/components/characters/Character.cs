using Godot;

public partial class Character : CharacterBody3D
{
    #region EXPORTS
    [ExportGroup("Movement Stats")]
    [Export] public float WALK_SPEED;
    [Export] public float RUN_SPEED;
    [Export] public float ROTATION_SPEED;
    [Export] public float ACCELERATION;
    [Export] public float DECELERATION;
    [Export] public float WALKING_DECELERATION_RANGE;
    [Export] public float RUNNING_DECELERATION_RANGE;

    [ExportGroup("")]
    [Export] protected NavigationManager _navigationManager;
    #endregion

    #region PUBLIC_VARIABLES
    public bool IsInteracting;
    public Node3D InteractionTarget;
    public float Speed;
    public float MaxSpeed;
    public bool IsRunning;
    #endregion


    #region VIRTUAL_METHODS
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (IsInteracting)
            FaceTowards(InteractionTarget.GlobalPosition, delta);
    }
    #endregion 

    #region PUBLIC_METHODS
    public void MoveCharacter(Vector3 targetPosition)
    {
        _navigationManager.SetNavigationPath(targetPosition);
    }

    public void Accelerate(double delta)
    {
        float decelerationRange;

        if (IsRunning)
        {
            MaxSpeed = RUN_SPEED;
            decelerationRange = RUNNING_DECELERATION_RANGE;
        } else
        {
            MaxSpeed = WALK_SPEED;
            decelerationRange = WALKING_DECELERATION_RANGE;
        }

        if (Speed < MaxSpeed && _navigationManager.DistanceToTarget() > decelerationRange)
            Speed = Mathf.MoveToward(Speed, MaxSpeed, (float)(ACCELERATION * delta));
        else if (_navigationManager.DistanceToTarget() <= decelerationRange)
            Decelerate(delta);
        
        if (Speed == 0)
            IsRunning = false;
    }

    public void FaceTowards(Vector3 target, double delta)
    {
        Vector3 _directionTargetPos = new Vector3(target.X, GlobalPosition.Y, target.Z);
        Transform3D newTransform = Transform.LookingAt(_directionTargetPos, Vector3.Up);
        Transform = Transform.InterpolateWith(newTransform, ROTATION_SPEED * (float)delta);
    }
    #endregion

    #region PRIVATE_METHODS
    private void Decelerate(double delta)
    {
        Speed = Mathf.MoveToward(Speed, 0, (float)(DECELERATION * delta));
    }
    #endregion
}