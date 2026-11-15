using Godot;
using System;

public partial class MainGame : Node2D
{
	[Export]
	public int MaxPillowNum = 3;
	public int CurPillowNum = 1;
	public int NumBuns = 0;
	public GlobalSound globalSound;
	public Bun bun1;
	public Bun bun2;
	public Bun bun3;
	public static int levelNum = 3;
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
		bun1 = GetNode<Bun>("Bun1");
		bun2 = GetNode<Bun>("Bun2");
		bun3 = GetNode<Bun>("Bun3");
		//bun1.Show();
		//bun2.Show();
		//bun3.Show();
		globalSound = GetNode<GlobalSound>("/root/GlobalSound");
		SetupLevel(); //comment out for testing levels
		
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
		GD.Print(GetNode<Node2D>("Hedgie").GlobalPosition);
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
		bun1.Show();
		bun2.Show();
		bun3.Show();
	}

	public async void OnLose() {
		GetNode<Sprite2D>("Baker").Position = new Vector2(2324, 576);
		GetNode<Sprite2D>("Baker").Show();
		globalSound.PlaySound("thump");
		await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);
		AnimationPlayer ani = GetNode<AnimationPlayer>("BakerAni");
		ani.Play("caught");
		globalSound.PlaySound("stone_roll");
		await ToSignal(ani, AnimationPlayer.SignalName.AnimationFinished);
		Sprite2D caughtSprite = GetNode<Sprite2D>("Caught");
		if (GetNode<Sprite2D>("Baker").Visible) {
			caughtSprite.Show();
		}
	}
	
	public void SetupLevel() {
		bun1.Show();
		bun2.Show();
		bun3.Show();
		switch (levelNum) {
			case 1: SetBunPos(360, 273, 578, 423, 757, 601); MoveLamp(-200, -200); break;
			case 2: SetBunPos(462, 188, 471, 506, 128, 574); MoveLamp(568, 167); break;
			case 3: SetBunPos(1406, 864, 607, 416, 1183, 371); MoveLamp(1384, 265); break;
		}
	}
	private void SetBunPos(int bun1X, int bun1Y, int bun2X, int bun2Y, int bun3X, int bun3Y) {
		bun1.Position = new Vector2(bun1X, bun1Y);
		bun2.Position = new Vector2(bun2X, bun2Y);
		bun3.Position = new Vector2(bun3X, bun3Y);
	}
	public void MoveLamp(int x, int y) {
		GetNode<StaticBody2D>("Lamp").Position = new Vector2(x, y);
	}
}
