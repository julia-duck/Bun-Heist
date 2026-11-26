using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class MainGame : Node2D
{
	[Export]
	public int MaxPillowNum = 3;
	public int CurPillowNum = 0;
	public List<bool> pillowsUsed;
	public int NumBuns = 0;
	public GlobalSound globalSound;
	public Bun bun1;
	public Bun bun2;
	public Bun bun3;
	public static int levelNum = 1;
	
	public static int LevelsUnlocked = 1;
	public static int MaxLevels = 7;
	public static bool tutorial = false;
	
	public Sprite2D bunCounter1;
	public Sprite2D bunCounter2;
	public Sprite2D bunCounter3;
	public Sprite2D thanksScreen;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pillowsUsed = new List<bool>();
		Sprite2D caughtSprite = GetNode<Sprite2D>("Caught");
		caughtSprite.Hide();
		bunCounter1 = GetNode<Sprite2D>("BunCounter/BunGet1");
		bunCounter2 = GetNode<Sprite2D>("BunCounter/BunGet2");
		bunCounter3 = GetNode<Sprite2D>("BunCounter/BunGet3");
		thanksScreen = GetNode<Sprite2D>("Thanks");
		thanksScreen.Hide();

		for (int i = 1; i <= MaxPillowNum; i++) {
			pillowsUsed.Add(false);
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
		/*for (int i = 1; i <= CurPillowNum; i++) {
			Pillow curPillow = GetNode<Pillow>("Pillow" + i);
			curPillow.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
			curPillow.Show();
		}
		for (int i = CurPillowNum + 1; i <= MaxPillowNum; i++) {
			Pillow curPillow = GetNode<Pillow>("Pillow" + i);
			curPillow.Disable();
		}*/
		for (int i = 0; i < MaxPillowNum; i++) {
			Pillow curPillow = GetNode<Pillow>("Pillow" + (i+1));
			if (pillowsUsed[i]){
				curPillow.Enable();
			}
			else {
				curPillow.Disable();
			}
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
			bool added = false;
			int index = 0;
			for (int i = 0; i < CurPillowNum && !added; i++) {
				if (!pillowsUsed[i]) {
					pillowsUsed[i] = true;
					index = i;
					added = true;
				}
			}
			if (!added){
				GD.Print("Pillow not added");
			}
			GetNode<Pillow>("Pillow" + (index+1)).OnStop();
		}
	}

	public void OnStartButtonClick()
	{
		OnRetreatButtonClick();
		GetNode<Button>("PillowButton").Disabled = true;
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
		GetNode<Button>("PillowButton").Disabled = false;
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
		{	
			globalSound.PlaySound("success");
			if (levelNum <= MaxLevels) {
				
				var complete = GetNode<Node2D>("Complete");
				complete.Show();
				Button pB = GetNode<Button>("PillowButton");
				Button startB = GetNode<Button>("StartButton");
				Button stopB = GetNode<Button>("StopButton");
				pB.Disabled = true;
				startB.Disabled = true;
				stopB.Disabled = true;
				levelNum ++;
				if (LevelsUnlocked < levelNum) {
					LevelsUnlocked = levelNum;
				}
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
	
	public void GetRidOfPillows() {
		for (int i = 0; i < MaxPillowNum; i++) {
			pillowsUsed[i] = false;
		}	
	}

	public void SetupLevel() {
		var complete = GetNode<Node2D>("Complete");
		Button pB = GetNode<Button>("PillowButton");
		Button startB = GetNode<Button>("StartButton");
		Button stopB = GetNode<Button>("StopButton");
		pB.Disabled = false;
		startB.Disabled = false;
		stopB.Disabled = false;
		complete.Hide();
		CurPillowNum = 0;
		GetRidOfPillows();
		NumBuns = 0;
		bun1.Show();
		bun2.Show();
		bun3.Show();
		if (levelNum == 1) {
			tutorial = true;
			GetNode<Node2D>("Tutorial").Show();
			Tutorial();
		}
		else {
			tutorial = false;
			GetNode<Node2D>("Tutorial").Hide();
		}
		switch (levelNum) {
			case 1:
				SetBunPos(360, 273, 578, 423, 757, 601);
				MoveOffScreen(true, true, true, true);
				break;
			case 2: 
				SetBunPos(462, 188, 471, 506, 128, 574);
				MoveOffScreen(false, true, true, true);
				MoveLamp(568, 167); 
				break;
			case 3: 
				SetBunPos(671, 859, 945, 663, 1236, 859);
				MoveOffScreen(true, true, true, true);
				break;
			case 4:
				SetBunPos(1200, 199, 1357, 204, 1509, 202);
				MoveOffScreen(true, true, true, true);
				break;
			case 5: 
				SetBunPos(1406, 864, 607, 416, 1183, 371); 
				MoveOffScreen(false, true, true, true); 
				MoveLamp(1384, 265); 
				break;
			case 6:
				SetBunPos(1778, 295, 928, 305, 1393, 638);
				MoveOffScreen(false, true, true, false); 
				MoveBox(926, 811);
				MoveLamp(1476, 152);
				break;
			case 7:
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
		GetNode<Node2D>("Lamp").Position = new Vector2(x, y);
	}
	public void MoveCounter1(int x, int y) {
		GetNode<Node2D>("Counter1").Position = new Vector2(x, y);
	}
	public void MoveCounter2(int x, int y) {
		GetNode<Node2D>("Counter2").Position = new Vector2(x, y);
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
			MoveBox(-500, -500);
		}
	}

	public async Task Tutorial()
	{
		GD.Print("Tutorial started");
		Button pillowButton = GetNode<Button>("PillowButton");

		GD.Print("Awaiting signal");//
		await ToSignal(pillowButton, Button.SignalName.Pressed);

		GD.Print("Hiding tutorial");//
		GetNode<Control>("Tutorial/AddTutorial").Visible = false;
		GD.Print("Pillow added");//

		Control placeTutorial = GetNode<Control>("Tutorial/PlaceTutorial");
		placeTutorial.Show();
	}
}
