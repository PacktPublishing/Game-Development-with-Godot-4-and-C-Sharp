using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class ForestDweller : CharacterBody3D
{
	private float speed = 1.5f;
	private NavigationAgent3D navAgent;

	public override void _Ready()
	{
		navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		GD.Print("Starting position is: " + this.GlobalPosition.ToString());
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector3 currLocation = GlobalTransform.Origin;
		Vector3 nextLocation = navAgent.GetNextPathPosition();
		Vector3 newVelocity = (nextLocation - currLocation).Normalized() * speed;

		this.Velocity = newVelocity;
		MoveAndSlide();

	}
	public void SetTarget(Vector3 targetPosition)
	{
		navAgent.TargetPosition = targetPosition;
	}
	public async void OnNavAgentTargetReached()
	{
		GD.Print("Position reached!");
		var root = GetOwner<World>();
		await ToSignal(GetTree().CreateTimer(3f), SceneTreeTimer.SignalName.Timeout);
		root.ChooseRandomPatrolPoint();
	}
}
