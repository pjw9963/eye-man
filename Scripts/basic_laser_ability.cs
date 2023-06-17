using System;
using System.Threading.Tasks;
using Godot;

public partial class basic_laser_ability : Node
{
	[Signal]
	public delegate void ShootEventHandler(Node2D projectile);

	private PackedScene
		_laser = GD.Load<PackedScene>("res://Scenes/Laser.tscn");

	public const int SHOOT_COOLDOWN = 250;

	private bool CAN_SHOOT;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CAN_SHOOT = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if (Input.IsActionPressed("Fire") && CAN_SHOOT)
		{
			var laserInstance = (Laser) _laser.Instantiate();
			laserInstance.Rotation = 3.14F;
			laserInstance.Position = new Vector2(500, 500);
			EmitSignal(SignalName.Shoot, laserInstance);

			// cooldown logic
			CAN_SHOOT = false;
			await Task.Delay(TimeSpan.FromMilliseconds(SHOOT_COOLDOWN));
			CAN_SHOOT = true;
		}
	}
}
