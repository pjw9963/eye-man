using System;
using System.Threading.Tasks;
using Godot;

public partial class EnemySpawner : Node
{
	[Export]
	public int waveThreshold = 100;

	[Signal]
	public delegate void SpawnEventHandler(PackedScene enemy, Vector2 location);

	private PackedScene _ufo = GD.Load<PackedScene>("res://Scenes/ufo.tscn");

	private double spawnPoints;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnPoints = 100;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (spawnPoints >= waveThreshold)
		{
			while (spawnPoints >= 20)
			{
				purchase();
			}
		}

		// regen spawn points for next wave
		spawnPoints += (8 * delta);
	}

	public async Task purchase()
	{
		var random = new Random();
		var viewport = GetViewport();
		var viewportSize = viewport.GetVisibleRect().Size;

		if (spawnPoints < 20)
			return;
		else
			spawnPoints -= 20;

		// get location
		var locaction = getOffScreenLocation();

		// get delay
		var spawnDelay = random.Next(100, 5000);

		// wait delay and then create instance of ufo at location
		await Task.Delay(TimeSpan.FromMilliseconds(spawnDelay));
		EmitSignal(SignalName.Spawn, _ufo, locaction);
	}

	private Vector2 getOffScreenLocation()
	{
		var random = new Random();
		var viewport = GetViewport();
		var viewportSize = viewport.GetVisibleRect().Size;

		Vector2 locaction = new Vector2();

		bool xAxis = random.Next(2) == 1;
		bool greaterThanZero = random.Next(2) == 1;

		// +/- 100 X or +/- 100 Y
		if (xAxis)
		{
			if (greaterThanZero)
				locaction.X = viewportSize.X + 100;
			else
				locaction.X = -100;

			locaction.Y = random.Next(0, (int) viewportSize.Y);
		}
		else
		{
			if (greaterThanZero)
				locaction.Y = viewportSize.Y + 100;
			else
				locaction.Y = -100;

			locaction.X = random.Next(0, (int) viewportSize.X);
		}

		return locaction;
	}
}
