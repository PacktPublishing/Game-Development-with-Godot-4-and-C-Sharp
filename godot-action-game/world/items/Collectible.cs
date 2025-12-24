using Godot;
using System;

public partial class Collectible : Node3D
{
	// Called when the node enters the scene tree for the first time.

	private Tween _tween;
	public override void _Ready()
	{
		_tween = CreateTween().SetLoops();
		_tween.TweenProperty(this, "position:y", .25f, 1f).AsRelative();
		_tween.TweenProperty(this, "rotation:x", ChooseRandomDegree(), 1f).AsRelative();
		_tween.TweenInterval(.2f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	private static float ChooseRandomDegree()
	{
		Random rDegree = new();
		float randomDegree = rDegree.Next(0,30); 
		return randomDegree;
	}

}
