using Godot;
using System;
using System.Threading.Tasks;

public partial class MainMenu : Control
{
	private AnimationPlayer animPlayer;
	private AudioStreamPlayer audioPlayer;
	private CanvasLayer mmCanvas;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		audioPlayer = GetNode<AudioStreamPlayer>("MainMenuTransition");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayClicked()
	{
		GD.Print("Play button clicked");
		animPlayer.Play("MenuTransition");
		audioPlayer.Play();
		HideMenu();
	}

	public async void HideMenu()
	{
		//await Task.Delay(TimeSpan.FromSeconds(1));
		await ToSignal(GetTree().CreateTimer(1f), SceneTreeTimer.SignalName.Timeout);
		this.Visible = false;
		//EmitSignal(SignalBus.SignalName.HideMainMenu);
		World root = GetParent<World>();
		root.PlayerStart();
		QueueFree();
	}

	public void ExitGame()
	{
		GetTree().Quit();
	}
	
	private void OnSettingsClicked()
	{
		Node settingsScene = ResourceLoader.Load<PackedScene>("res://user_interface/Settings.tscn").Instantiate();
		AddChild(settingsScene);
	}
}
