using Godot;
using System;

public partial class MainGame : Node2D
{
	[Export]
	public int MaxPillowNum = 3;
	public int CurPillowNum = 0;
	public int NumBuns = 0;
	public GlobalSound globalSound;
	public Bun bun1;
	public Bun bun2;
	public Bun bun3;
	public static int levelNum = 4;
	public static int MaxLevels = 5;
	
	public Sprite2D bunCounter1;
	public Sprite2D bunCounter2;
	public Sprite2D bunCounter3;
	public Sprite2D thanksScreen;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Sprite2D caughtSprite = GetNode<Sprite2D>("Caught");
		caughtSprite.Hide();
		bunCounter1 = GetNode<Sprite2D>("BunCounter/BunGet1");
		bunCounter2 = GetNode<Sprite2D>("BunCounter/BunGet2");
		bunCounter3 = GetNode<Sprite2D>("BunCounter/BunGet3");
		thanksScreen = GetNode<Sprite2D>("Thanks");
		thanksScreen.Hide();

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
		bunCounter1.Show();
		bunCounter2.Show();
		bunCounter3.Show();
		if (NumBuns == 0) {
			bunCounter1.Hide();
			bunCounter2.Hide();
			bunCounter3.Hide();
		}
		else if (NumBuns == 1) {
			bunCounter2.Hide();
			bunCounter3.Hide();
		}
		else if (NumBuns == 2) {
			bunCounter3.Hide();
		}
	}
	
	public void OnPillowButtonClick() {
		//give new pillow
		if (CurPillowNum < MaxPillowNum) {
			CurPillowNum += 1;
			GetNode<Pillow>("Pillow" + CurPillowNum).OnStop();
		}
	}

	public void OnRetreatButtonClick() {
		GetNode<Node2D>("Unfinished").Hide();
		GetNode<Sprite2D>("Baker").Hide();
		GetNode<Sprite2D>("Caught").Hide();
		//enable 3 buns collision shape
		bun1.EnableShape();
		bun2.EnableShape();
		bun3.EnableShape();
		NumBuns = 0;
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
	
	public async void OnLevelComplete(Node2D hedgie)
	{
		
		if (NumBuns == 3)
		{	if (levelNum <= MaxLevels) {

				var complete = GetNode<Node2D>("Complete");
				complete.Show();
				levelNum ++;
				var button = complete.GetNode<Button>("Button");
				await ToSignal(button, Button.SignalName.Pressed);
				SetupLevel();
				complete.Hide();
				CurPillowNum = 0;
			}
			else {
				thanksScreen.Show();
			}
		}
		else
		{
			GetNode<Node2D>("Unfinished").Show();
		}
		
		
	}

	public void SetupLevel() {
		NumBuns = 0;
		bun1.Show();
		bun2.Show();
		bun3.Show();
		switch (levelNum) {
			case 1:
				SetBunPos(360, 273, 578, 423, 757, 601);
				MoveOffScreen(true, true, true, true);
				GetNode<Node2D>("Tutorial").Show();
				break;
			case 2: 
				SetBunPos(462, 188, 471, 506, 128, 574);
				MoveOffScreen(false, true, true, true);
				MoveLamp(568, 167); 
				GetNode<Node2D>("Tutorial").Hide();
				break;
			case 3: 
				SetBunPos(1406, 864, 607, 416, 1183, 371); 
				MoveOffScreen(false, true, true, true); 
				MoveLamp(1384, 265); 
				break;
			case 4:
				SetBunPos(1778, 295, 928, 305, 1393, 638);
				MoveOffScreen(false, true, true, false); 
				MoveBox(926, 811);
				MoveLamp(1476, 152);
				break;
			case 5:
				SetBunPos(450, 604, 936, 315, 1662, 260);
				MoveOffScreen(true, true, false, true);
				MoveCounter1(1442, 1145);
				break;
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
	public void MoveCounter1(int x, int y) {
		GetNode<Sprite2D>("Counter1").Position = new Vector2(x, y);
	}
	public void MoveCounter2(int x, int y) {
		GetNode<Sprite2D>("Counter2").Position = new Vector2(x, y);
	}
	public void MoveBox(int x, int y) {
		GetNode<Node2D>("Box2").Position = new Vector2(x, y);
	}
	public void MoveOffScreen(bool lampOff, bool c1Off, bool c2Off, bool boxOff) {
		if (lampOff) {
			MoveLamp(-200, -200);
		}
		if (c1Off) {
			MoveCounter1(-400, -400);
		}
		if (c2Off) {
			MoveCounter2(-400, -400);
		}
		if (boxOff) {
			MoveBox(-300, -300);
		}
	}
}
