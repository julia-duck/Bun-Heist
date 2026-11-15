using Godot;
using System;
using System.Threading.Tasks;

public partial class TitleScreen : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public async override void _Ready()
	{
		await ToSignal(GetTree().CreateTimer(3f), SceneTreeTimer.SignalName.Timeout);
		GetNode<ColorRect>("ColorRect").Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public async void OnBeginPressed()
	{
		GetNode<Button>("Button").Hide();
		GetNode<Node2D>("Start").Hide();
		GetNode<Node2D>("Title").Hide();
		var opening = GetNode<AnimatedSprite2D>("Opening");
		opening.Play();
		await ToSignal(opening, AnimatedSprite2D.SignalName.AnimationFinished);
		GetTree().ChangeSceneToFile("res://scenes/main_game.tscn");

	}
}
