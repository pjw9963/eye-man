using System;
using System.Threading.Tasks;
using Godot;

public partial class UFOCharacterBody2D : CharacterBody2D
{
	[Signal]
	public delegate void
		ShootEventHandler(PackedScene laser, float direction, Vector2 location);

	private PackedScene
		_laser = GD.Load<PackedScene>("res://Scenes/Laser.tscn");

	private Vector2 targetPos;

	private Vector2 sourcePos;

	private int lerpWeight = 25;

	public int Speed = 200;

	private bool CAN_GET_NEW_TARGET;

	private bool WAITING_TO_MOVE;

	private int NEW_TARGET_COOLDOWN = 2000;

	private Random random;

	private float movementTimer = 0;

	private bool ON_SCREEN;

	private bool CAN_SHOOT;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ON_SCREEN = false;
		random = new Random(); // use a seed for better random
		setTargetPos();
		CAN_GET_NEW_TARGET = false;
		WAITING_TO_MOVE = false;
		CAN_SHOOT = false;
		// wait till on screen to shoot
	}

	public override async void _PhysicsProcess(double delta)
	{
		movementTimer += (float) delta;
		Velocity = (targetPos - Position).Normalized() * Speed;
		float retX =
			Mathf.Lerp(Position.X, targetPos.X, movementTimer / lerpWeight);
		float retY =
			Mathf.Lerp(Position.Y, targetPos.Y, movementTimer / lerpWeight);

		// Velocity = new Vector2(Velocity.X * retX, Velocity.Y * retY);
		if ((targetPos - Position).Length() > 5)
		{
			Position = new Vector2(retX, retY);
		}
		else if (CAN_GET_NEW_TARGET)
		{
			setTargetPos();
		}
		else if (!WAITING_TO_MOVE)
		{
			// no longer moving, start cooldown
			var mc = movementCooldown();
		}

		if (CAN_SHOOT)
		{
			var player = GetTree().Root.GetNode<CharacterBody2D>("root/Player");
			EmitSignal(SignalName.Shoot,
			_laser,
			(player.Position - Position).Angle() + random.NextDouble() * 0.5 - 0.25,
			Position);
			shootCooldown();
		}
	}

	public void setSourcePos()
	{
		var windowSize = GetViewport().GetWindow().Size;
		CAN_GET_NEW_TARGET = false;
		targetPos =
			new Vector2(random.Next(100, windowSize.X),
				random.Next(100, windowSize.Y));
	}

	public void setTargetPos()
	{
		movementTimer = 0;
		var windowSize = GetViewport().GetWindow().Size;
		CAN_GET_NEW_TARGET = false;
		targetPos =
			new Vector2(random.Next(100, windowSize.X),
				random.Next(100, windowSize.Y));
	}

	public async Task movementCooldown()
	{
		WAITING_TO_MOVE = true;
		await Task.Delay(TimeSpan.FromMilliseconds(NEW_TARGET_COOLDOWN));
		WAITING_TO_MOVE = false;
		CAN_GET_NEW_TARGET = true;
	}

	public async Task shootCooldown()
	{
		CAN_SHOOT = false;
		await Task.Delay(TimeSpan.FromMilliseconds(NEW_TARGET_COOLDOWN));
		CAN_SHOOT = true;
	}

	private void _on_visible_on_screen_notifier_2d_screen_entered()
	{
		ON_SCREEN = true;
		CAN_SHOOT = true;
	}
}
