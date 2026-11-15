using Godot;
using System;

public partial class Bun : Area2D
{
	public GlobalSound globalSound;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		globalSound = GetNode<GlobalSound>("/root/GlobalSound");
		Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBodyEntered(Node2D hedgie)
	{
		Hide();
		CollisionShape2D colShape = GetNode<CollisionShape2D>("CollisionShape2D");
		SetDeferred("disabled", true);
		MainGame par = GetNode<MainGame>("..");
		par.NumBuns++;
		globalSound.PlaySound("collect_bun");
	}
}
