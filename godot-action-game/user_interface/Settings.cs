using Godot;
using System;
using System.Collections.Generic;

public partial class Settings : Control
{
	private CanvasLayer canvas;
	public int sfxBus;
	public int musicBus;

	private int audioTab = 0;
	private int controlTab = 1;

	private Control audioMenu;
	private Control controlMenu;

	private Button selectedButton;

	private Button leftKey;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		canvas = GetNode<CanvasLayer>("CanvasLayer");
		canvas.Visible = true;
		sfxBus = AudioServer.GetBusIndex("SFX");
		GD.Print(sfxBus);
		musicBus = AudioServer.GetBusIndex("Music");
		GD.Print(musicBus);
		GD.Print(AudioServer.GetBusChannels(0));

		audioMenu = GetNode<Panel>("CanvasLayer/AudioPanel");
		controlMenu = GetNode<Panel>("CanvasLayer/ControlsPanel");

		leftKey = GetNode<Button>("CanvasLayer/ControlsPanel/VBoxContainer/Left");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
		if(controlMenu.Visible)
		{
			if(@event is InputEventKey eventKey)
			{
				if(eventKey.Pressed)
				{
					List<String> allMappedActions = new();
					foreach(var allActions in InputMap.GetActions())
					{
						foreach(var allThings in InputMap.ActionGetEvents(allActions))
						{
							//allMappedActions[allThings.AsText()] = allActions;
						}
					}
				}

				InputMap.ActionEraseEvents(selectedButton.Name);

				InputMap.ActionAddEvent(selectedButton.Name, eventKey);

				selectedButton = null;
				GD.Print("Key bound.");
			}
		}
    }


	private void OnSettingsClose()
	{
		//canvas.Visible = false;
		this.QueueFree();
	}

	private void ChangeMusicVolume(double value)
	{
		AudioServer.SetBusVolumeDb(musicBus, (float)Mathf.LinearToDb(value));
	}

	private void ChangeSFXVolume(double value)
	{
		AudioServer.SetBusVolumeDb(sfxBus, (float)Mathf.LinearToDb(value));
	}

	private void TabChanged(int tab)
	{
		switch(tab)
		{
			case 0:
				audioMenu.Visible = true;
				controlMenu.Visible = false;
				break;
			case 1:
				audioMenu.Visible = false;
				controlMenu.Visible = true;
				break;
		}
	}

	private void OnButtonPressed()
	{
		GD.Print("Button pressed!");
		selectedButton = leftKey;
	}
}
