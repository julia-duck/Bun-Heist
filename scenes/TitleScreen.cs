using Godot;
using System;
using System.Threading.Tasks;

public partial class TitleScreen : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public async void OnBeginPressed()
	{
		GetNode<Button>("Button").Hide();
		GetNode<Node2D>("Title").Hide();
		var opening = GetNode<AnimatedSprite2D>("Opening");
		opening.Play();
		await ToSignal(opening, AnimatedSprite2D.SignalName.AnimationFinished);
		GetTree().ChangeSceneToFile("res://scenes/main_game.tscn");

	}
}
