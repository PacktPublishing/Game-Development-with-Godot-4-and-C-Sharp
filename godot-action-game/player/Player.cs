using Godot;
using System;


public partial class Player : CharacterBody3D
{
	enum PlayerState
	{
		IDLE,
		WALKING,
		JUMPING,
		FALLING,
		RUNNING
	}

	private PlayerState currPlayerState;

	public const float runSpeed = 8.0f;
	public const float walkSpeed = 5.0f;
	public const float JumpVelocity = 4.5f;

	//You have to Build for exported variables to show up in the editor after adding the keyword
	//Panning the camera variables
	[Export(PropertyHint.Range,"0,0.5")]
	private float CameraSensitivity_H = 0.05f;
	[Export(PropertyHint.Range, "0,0.5")]
	private float CameraSensitivity_V = 0.05f;

	private AnimationTree anim;
	private Node3D body;
	private Node3D cameraPivot;
	private Vector3 rotation;
	private CollisionShape3D capsule;
	private Vector3 maxSpringRotation = new(30,30,0);
	private bool jumping = false;
	private float currSpeed = walkSpeed;

	private float horizontalJumpSpeed = 3f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready()
	{
		anim = GetNode<AnimationTree>("AnimationTree");
		body = GetNode<Node3D>("%Body");
		rotation = body.Rotation;
		cameraPivot = GetNode<Node3D>("%CameraPivot");
		capsule = GetNode<CollisionShape3D>("CollisionShape3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	public override void _Input(InputEvent @event)
	{
		//Camera Movement
		if(@event is InputEventMouseMotion eventMouseMotion)
		{	
			//rotates the camera based on our mouse position and sensitivity
			cameraPivot.RotateY(ConvertDegreesToRadian(-eventMouseMotion.Relative.X*CameraSensitivity_H));
			cameraPivot.RotateX(ConvertDegreesToRadian(-eventMouseMotion.Relative.Y*CameraSensitivity_V));
			
			//keeps the camera from turning wonky in both axis
			cameraPivot.Rotation = cameraPivot.Rotation.Clamp(-maxSpringRotation, maxSpringRotation);
		}	
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		switch(currPlayerState)
		{
			case PlayerState.WALKING:
				break;
			case PlayerState.RUNNING:
				break;
			case PlayerState.JUMPING:
				break;
			case PlayerState.FALLING:
				break;
			case PlayerState.IDLE:
				break;
		}

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y -= gravity * (float)delta;
		}

		//Reset jump
		if(IsOnFloor())
		{
			jumping = false;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			jumping = true;
			currPlayerState = PlayerState.JUMPING;
		}

		//Handle running
		if(Input.IsActionPressed("run") && IsOnFloor())
		{
			currSpeed = runSpeed;
			currPlayerState = PlayerState.RUNNING;
		}

		if(Input.IsActionJustReleased("run") && IsOnFloor())
		{
			currSpeed = walkSpeed;
			currPlayerState = PlayerState.WALKING;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		//Makes sure our character is always going the right dir relative to our camera's position on the Y-axis
		direction = direction.Rotated(Vector3.Up, cameraPivot.Rotation.Y);

		if (direction != Vector3.Zero && IsOnFloor())
		{
			velocity.X = direction.X * currSpeed;
			velocity.Z = direction.Z * currSpeed;

			rotation.Y = Mathf.LerpAngle(rotation.Y, Mathf.Atan2(velocity.X, velocity.Z), 0.15f); 
		}
		else 
		{
			if(jumping == false)
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, currSpeed);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, currSpeed);
			}
		}

		body.Rotation = rotation;
		Velocity = velocity;

		CheckForCollectibleCollision();

		//Animation Switching logic
		anim.Set("parameters/conditions/idle", (IsOnFloor() && inputDir == Vector2.Zero));
		anim.Set("parameters/conditions/move", (IsOnFloor() && inputDir != Vector2.Zero && currSpeed == walkSpeed));
		anim.Set("parameters/conditions/falling", (!IsOnFloor()));
		anim.Set("parameters/conditions/landing", (IsOnFloor()));
		anim.Set("parameters/conditions/running", (IsOnFloor() && currSpeed == runSpeed));
	
		MoveAndSlide();

	}

	public void CheckForCollectibleCollision()
	{
		for(int index=0; index < GetSlideCollisionCount(); index++)
		{
			KinematicCollision3D collision = GetSlideCollision(index);
			//GD.Print(collision.GetCollider());

			if(collision.GetCollider() is CollectibleTrigger collectible)
			{
				GD.Print("Collided with collectible");
				collectible.PickUp();
			}
		}
	}

	public static float ConvertDegreesToRadian(float num)
	{
		return num*((float)Math.PI/180);
	}
}
