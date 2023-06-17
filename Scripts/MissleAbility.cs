using System;
using System.Threading.Tasks;
using Godot;

public enum Side {
	Left = 0,
	Right = 1
}

public partial class MissleAbility : Node
{
	[Signal]
	public delegate void ShootEventHandler(Missle projectile, Side side);

	private PackedScene
		_missle = GD.Load<PackedScene>("res://Scenes/Missle.tscn");

	public const int RELOAD_COOLDOWN = 1000;

	private bool RELOADING = false;

	private const int MAX_AMMO = 4;

	private int ammo;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ammo = MAX_AMMO;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Fire") && ammo > 0)
		{
			ammo -= 1;
			// spawn missle and signal to player to lauch it
			var missleInstance = (Missle) _missle.Instantiate();
			if (ammo % 2 == 1)
			EmitSignal(SignalName.Shoot, missleInstance, (int) Side.Left);
			else
			EmitSignal(SignalName.Shoot, missleInstance, (int) Side.Right);
		}

		if (!RELOADING && ammo == 0)
		{
			RELOADING = true;
			await Task.Delay(TimeSpan.FromMilliseconds(RELOAD_COOLDOWN));
			ammo = 4;
			RELOADING = false;
		}
	}
}
