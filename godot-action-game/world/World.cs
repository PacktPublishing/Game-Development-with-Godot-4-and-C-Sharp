using Godot;
using System;
using System.Collections.Generic;

public partial class World : Node3D
{
	[Export] PackedScene _menu;

	[Signal]
	public delegate void CreatePatrolEventHandler();
	public SignalBus _signals;
	public int numOfMushrooms;
	public bool moveNPC = true;
	private MushroomCount mushroomLabel;

	private Camera3D menuCamera;
	private CharacterBody3D player;
	private AudioStreamPlayer worldMusic;
	private Vector3 currPatrolPoint;
	//private CharacterBody3D npc;
	private List<Vector3> patrolPoints = new();
	private int patrolNum = 0;

	private Node pauseScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		numOfMushrooms = 0;

		//_signals.HideMainMenu += PlayerStart;
		/*SignalBus.Instance.Connect(SignalBus.SignalName.HideMainMenu, new Callable(this, MethodName.CreatePatrolPath));
		if(SignalBus.Instance.IsConnected(SignalBus.SignalName.HideMainMenu, new Callable(this, MethodName.CreatePatrolPath)))
		{
			GD.Print("We connected!");
		}*/
		//Node mainMenuScene = ResourceLoader.Load<PackedScene>("res://user_interface/MainMenu.tscn").Instantiate();
		//AddChild((Control)mainMenuScene);
		
		CreatePatrol += () => CreatePatrolPath();
		
		var loadMenu = _menu.Instantiate();
		AddChild(loadMenu);

		menuCamera = GetNode<Camera3D>("MenuCamera");
		player = GetNode<CharacterBody3D>("Player");
		worldMusic = GetNode<AudioStreamPlayer>("WorldMusic");
		patrolPoints.Add(GetNode<Marker3D>("NPC/Patrol1").GlobalPosition);
		patrolPoints.Add(GetNode<Marker3D>("NPC/Patrol2").GlobalPosition);
		patrolPoints.Add(GetNode<Marker3D>("NPC/Patrol3").GlobalPosition);
		patrolPoints.Add(GetNode<Marker3D>("NPC/Patrol4").GlobalPosition);

		if(menuCamera.Current)
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
			player.ProcessMode = ProcessModeEnum.Disabled;
			moveNPC = false;
		}
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		//mushroomLabel.mushroomAmount += numOfMushrooms;
		
		if(Input.IsActionJustPressed("ui_cancel"))
		{
			GD.Print("Escape key pressed - pause game");
			pauseScene = ResourceLoader.Load<PackedScene>("res://user_interface/PauseMenu.tscn").Instantiate();
			AddChild(pauseScene);
			GetTree().Paused = true;
			Input.MouseMode = Input.MouseModeEnum.Visible;
			player.ProcessMode = ProcessModeEnum.Disabled;
		}
	}

	public void PlayerStart()
	{
		GD.Print("About to play music");
		worldMusic.Play();
		menuCamera.Current = false;
		player.ProcessMode = ProcessModeEnum.Always;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		moveNPC = true;
		if(SignalName.CreatePatrol != null)
		{
			EmitSignal(SignalName.CreatePatrol);
		}
	}

	public void ChooseRandomPatrolPoint()
	{
		Random rInt = new();
		int prevPatrolNum = patrolNum;
		//prevents the NPC from pathing to the same place or trying to
		while(prevPatrolNum == patrolNum)
		{
			patrolNum = rInt.Next(0,3);
		}
		CreatePatrolPath();
	}

	public void CreatePatrolPath()
	{
		GD.Print("Setting new target position.");
		GetTree().CallGroup("npc", "SetTarget", patrolPoints[patrolNum]);
	}
}
