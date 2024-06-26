using Godot;
using System;
using System.Reflection;

public partial class CollectibleTrigger : StaticBody3D
{

	[Signal]
	public delegate void CollectMushroomEventHandler();

	//private Label mushroomLabel;
	private Node3D world;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//mushroomLabel = GetNode<Label>("%MushroomCount");
	}

	public void PickUp()
	{
		//Increase collectible amount on UI here
		EmitSignal(SignalName.CollectMushroom);
		this.GetParent().QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		
	}

}
