using Godot;
using System;

public partial class LevelNum : Button
{
	[Export]
	public int ButtonNum; 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void OnButtonPressed() {
		GetNode<LevelSelectScreen>("..").LevelSelected = ButtonNum;
	}
}
