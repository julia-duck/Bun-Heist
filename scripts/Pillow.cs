using Godot;
using System;

public partial class Pillow : StaticBody2D
{
	public int frameNum = 1;
	public bool follow = false;
	public bool moveable = true;
	public bool rotate = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InputPickable = true;
		moveable = true;
		rotate = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (follow) {
			if (Input.IsMouseButtonPressed(MouseButton.Left)) {
				GlobalPosition = GetViewport().GetMousePosition();
			}
			else {
				follow = false;
			}
		}
		if (rotate && moveable)
		{
			var mousePos = GetViewport().GetMousePosition();
			var deltaY = mousePos.Y - Position.Y;
			var deltaX = mousePos.X - Position.X;
			var angle =  -Math.Atan(deltaX / deltaY);
			if (deltaY > 0) //if the mouse is above
			{
				angle -= Math.PI;
			}
			Rotation = (float) angle;
		}
		GetNode<Sprite2D>("Sprite2D").Frame = frameNum;
	}
	public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx) {
		if (@event is InputEventMouseButton mouse && moveable) {
			Vector2 mousePos = GetViewport().GetMousePosition();
			Vector2 pillowPos = GlobalPosition;
			if (mouse.Pressed && mousePos.X >= pillowPos.X - 150 && mousePos.X <= pillowPos.X + 150 && mousePos.Y >= pillowPos.Y - 50 && mousePos.Y <= pillowPos.Y + 50) {
				follow = true;
			}
		}
	}
	public void SetRandFrame(int num) {
		frameNum = num;
	}
	public void OnStart() {
		moveable = false;
		GetNode<Node2D>("Buttons").Hide();
	}

	public void OnStop() {
		moveable = true;
		GetNode<Node2D>("Buttons").Show();
	}
	
	public void OnRotateButtonDown()
	{
		rotate = true;
	}
	public void OnRotateButtonUp()
	{
		rotate = false;
	}
	public void OnDeletePressed()
	{
		if (moveable)
		{
			Disable();
			GetNode<MainGame>("..").CurPillowNum --;
		}
	}
	public void Disable()
	{
		Hide();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
	}
}
