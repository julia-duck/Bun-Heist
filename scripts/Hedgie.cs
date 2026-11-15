using Godot;
using System;

public partial class Hedgie : CharacterBody2D
{
	public const float Speed = 300.0f;
	public float Gravity = 0.0003F;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
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
			if (collider is StaticBody2D pillow) {
				Velocity = info.GetNormal() * new Vector2(1000, 1000); //change vector2 to make it even bouncier
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
}
