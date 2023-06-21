using System;
using System.Threading.Tasks;
using Godot;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 500;

	// public delegate void ShootEventHandler(PackedScene laser, float direction, Vector2 location);
	[Signal]
	public delegate void ShootEventHandler(Node projectile);

	private PackedScene
		_laser = GD.Load<PackedScene>("res://Scenes/Laser.tscn");

	private Node2D _diodeNode;

	public const int SHOOT_COOLDOWN = 250;

	private bool canShoot;

	public override void _Ready()
	{
		base._Ready();
		canShoot = true;
		_diodeNode = GetNode<Node2D>("Diode");
	}

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
		Velocity = inputDirection * Speed;
		if (
			Input.IsActionPressed("Left") ||
			Input.IsActionPressed("Right") ||
			Input.IsActionPressed("Up") ||
			Input.IsActionPressed("Down")
		) Rotation = inputDirection.Angle();
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}

	public void _on_projectile_fired(Node2D projectile)
	{
		projectile.Rotation = Rotation;
		projectile.Position = _diodeNode.GlobalPosition;
		EmitSignal(SignalName.Shoot, projectile);
	}

	public void _on_missile_fired(Missle missle, Side side)
	{
		// set velocity to left/right of forward position
		var offset = side == Side.Left ? -1 * MathF.PI / 2 : MathF.PI / 2;
		var missleDirection = offset + Rotation;
		missle.Velocity =
			new Vector2((float) Math.Cos(missleDirection),
				(float) Math.Sin(missleDirection)) *
			150;
		missle.Position = Position;
		missle.Rotation = missleDirection;
		EmitSignal(SignalName.Shoot, missle);
	}
}
