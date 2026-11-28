using Godot;
using System;

public partial class Settings : CanvasLayer
{
	private bool active = false;
	private Control panel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		panel = GetNode<Control>("Panel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnSettingsPressed()
	{
		if (active)
		{
			panel.Hide();
			active = false;
		}
		else
		{
			panel.Show();
			active = true;
		}
	}
	public void OnLevelSelectPressed()
	{
		GetParent().GetNode<CanvasLayer>("LevelSelectScreen").Show();
	}
}
