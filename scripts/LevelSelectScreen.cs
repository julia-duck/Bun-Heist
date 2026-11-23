using Godot;
using System;

public partial class LevelSelectScreen : CanvasLayer
{
	public int LevelSelected = 1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("select_level")) {
			Show();
		}
	}
	public void OnSelectLevel() {
		MainGame.levelNum = LevelSelected;
		GetNode<MainGame>("..").SetupLevel();
		Hide();
	}
	public void OnExit() {
		Hide();
	}
	
}
