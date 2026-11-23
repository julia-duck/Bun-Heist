using Godot;
using System;

public partial class GlobalSound : Node
{
	private AudioStreamPlayer SoundPlayer;

	private AudioStream collect_bun, bounce, success, stone_roll, thump;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SoundPlayer = GetNode<AudioStreamPlayer>("SoundEffectPlayer");

		collect_bun = ResourceLoader.Load<AudioStream>("res://assets/sound/collect_bun.wav");
		bounce = ResourceLoader.Load<AudioStream>("res://assets/sound/pillow_bounce.wav");
		success = ResourceLoader.Load<AudioStream>("res://assets/sound/success.wav");
		stone_roll = ResourceLoader.Load<AudioStream>("res://assets/sound/stone_roll.wav");
		thump = ResourceLoader.Load<AudioStream>("res://assets/sound/thump.wav");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// Plays the specified sound.
	/// </summary>
	/// <param name="sound">The name of the sound (probably the name of the file without .wav)</param>
	public void PlaySound(String sound)
	{
		var stream = (AudioStream) (GodotObject) Get(sound);
		if (stream != null)
		{
			SoundPlayer.Stream = stream;
			SoundPlayer.Play();
		}
	}
}
