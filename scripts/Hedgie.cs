using Godot;
using System;
using System.Numerics;

public partial class Hedgie : CharacterBody2D
{
	[Signal]
	public delegate void LoseEventHandler();
	[Export]
	public int StartX = 0;
	[Export]
	public int StartY = 0;
	[Export]
	public int StartVelocity = 500;
	[Export] //unexport later
	public int CushionBounciness = 1000;
	public const float Speed = 300.0f;
	public float Gravity = 0.0003F;
	public bool EnablePhysics = false;
	public GlobalSound globalSound;

	public override void _Ready()
	{
		globalSound = GetNode<GlobalSound>("/root/GlobalSound");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (EnablePhysics)
		{
			Godot.Vector2 velocity = Velocity;
			// Add the gravity.
			if (!IsOnFloor())
			{
				velocity += GetGravity() * (float)delta;
			}
			Velocity = velocity;
			MoveAndSlide();
			int collisionCount = GetSlideCollisionCount();
			for (int i = 0; i < collisionCount; i++) {
				var info = GetSlideCollision(i);
				var collider = info.GetCollider();

				if (collider is StaticBody2D body)
				{
					var colliderLayer = body.GetCollisionLayer();
					//pillow
					if (colliderLayer == 8) { //1000 = layer 4
						Velocity += info.GetNormal() * new Godot.Vector2(CushionBounciness, CushionBounciness);
						globalSound.PlaySound("bounce");
					}
					//floor
					else if (colliderLayer == 16) //10000 = layer 5
					{
						EnablePhysics = false;
						Velocity = Godot.Vector2.Zero;
						EmitSignal(SignalName.Lose);
					}
				}
			}
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		//Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		//if (direction != Vector2.Zero)
		//{
			//velocity.X = direction.X * Speed;
		//}
		//else
		//{
			//velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		//}

	}

	public void OnStartPhysics()
	{
		EnablePhysics = true;
		Velocity = new Godot.Vector2(StartVelocity, 0);
	}
	public void OnStopPhysics()
	{
		EnablePhysics = false;
		Position = new Godot.Vector2(StartX, StartY);
	}
	public void OnFinishLevel(Node2D player)
	{
		globalSound.PlaySound("success");
		EnablePhysics = false;
	}
}
