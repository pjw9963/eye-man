using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;
	[Signal]
	public delegate void ShootEventHandler(PackedScene laser, float direction, Vector2 location);
	private PackedScene _laser = GD.Load<PackedScene>("res://Scenes/Laser.tscn");
	private Node2D _diodeNode;

	public override void _Ready()
	{
		base._Ready();

		_diodeNode = GetNode<Node2D>("Diode");
	}

	public void GetInput()
	{

		Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
		Velocity = inputDirection * Speed;
		if (Input.IsActionJustPressed("Left") ||
			Input.IsActionJustPressed("Right") ||
			Input.IsActionJustPressed("Up") ||
			Input.IsActionJustPressed("Down"))
			Rotation = inputDirection.Angle();
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("Fire"))
		{
			EmitSignal(SignalName.Shoot, _laser, Rotation, _diodeNode.GlobalPosition);
		}
	}
}
