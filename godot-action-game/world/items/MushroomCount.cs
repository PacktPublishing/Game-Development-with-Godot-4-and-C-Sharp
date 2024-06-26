using Godot;
using System;

public partial class MushroomCount : Label
{
	// Called when the node enters the scene tree for the first time.
	public int mushroomAmount = 0;

	public override void _Ready()
	{
		Text = $"Mushrooms: {mushroomAmount}";
	}

	public void OnMushroomPickedUp()
	{
		mushroomAmount += 1;
		Text = $"Mushrooms: {mushroomAmount}";
	}
}
