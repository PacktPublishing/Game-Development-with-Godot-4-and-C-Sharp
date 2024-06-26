using Godot;
using System;

public partial class SignalBus : Node
{
	public static SignalBus Instance { get; private set; }

	[Signal]
	public delegate void HideMainMenuEventHandler();
	// Called when the node enters the scene tree for the first time.

	public override void _EnterTree()
	{
		if (Instance != null)
        {
            GD.PushWarning("Attempted to re-create another instance of signal bus!");
            return;
        }

        Instance = this;
	}
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
