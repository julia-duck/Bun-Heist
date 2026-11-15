using Godot;
using System;

public partial class MainGame : Node2D
{
	[Export]
	public int MaxPillowNum = 3;
	public int CurPillowNum = 1;
	public int NumBuns = 0;
	public GlobalSound globalSound;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Sprite2D caughtSprite = GetNode<Sprite2D>("Caught");
		caughtSprite.Hide();
		for (int i = 1; i <= MaxPillowNum; i++) {
			int rand = GD.RandRange(0, 4);
			GetNode<Pillow>("Pillow" + i).SetRandFrame(rand);
			GetNode<Pillow>("Pillow" + i).Show();
		}
		globalSound = GetNode<GlobalSound>("/root/GlobalSound");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//show/hide pillows based on current number
		for (int i = 1; i <= CurPillowNum; i++) {
			Pillow curPillow = GetNode<Pillow>("Pillow" + i);
			curPillow.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
			curPillow.Show();
		}
		for (int i = CurPillowNum + 1; i <= MaxPillowNum; i++) {
			Pillow curPillow = GetNode<Pillow>("Pillow" + i);
			curPillow.Disable();
		}
	}
	
	public void OnPillowButtonClick() {
		//give new pillow
		if (CurPillowNum < MaxPillowNum) {
			CurPillowNum += 1;
		}
	}

	public void OnRetreatButtonClick() {
		GetNode<Sprite2D>("Baker").Hide();
		GetNode<Sprite2D>("Caught").Hide();
	}

	public async void OnLose() {
		globalSound.PlaySound("thump");
		await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);
		GetNode<Sprite2D>("Baker").Show();
		AnimationPlayer ani = GetNode<AnimationPlayer>("BakerAni");
		ani.Play("caught");
		globalSound.PlaySound("stone_roll");
		await ToSignal(ani, AnimationPlayer.SignalName.AnimationFinished);
		Sprite2D caughtSprite = GetNode<Sprite2D>("Caught");
		if (GetNode<Sprite2D>("Baker").Visible) {
			caughtSprite.Show();
		}
	}
}
