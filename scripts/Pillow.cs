using Godot;
using System;

public partial class Pillow : StaticBody2D
{
	public int frameNum = 1;
	public bool follow = false;
	public bool moveable = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InputPickable = true;
		moveable = true;
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
	}
	
}
