using System;
using System.Threading.Tasks;
using Godot;

public partial class root : Node2D
{
	public async void _on_player_shoot(
		PackedScene laser,
		float direction,
		Vector2 location
	)
	{
		// spawn laser and set rotation and direction
		var laserInstance = (Laser) laser.Instantiate();
		laserInstance.Rotation = direction;
		laserInstance.Position = location;
		AddChild (laserInstance);
	}

	private void _on_ufo_shoot(
		PackedScene laser,
		float direction,
		Vector2 location
	)
	{
		// spawn laser and set rotation and direction
		var laserInstance = (Laser) laser.Instantiate();
		laserInstance.Rotation = direction;
		laserInstance.Position = location;
		AddChild (laserInstance);
	}

	private void _on_enemy_spawner_spawn(PackedScene enemy, Vector2 location)
	{
		// spawn laser and set rotation and direction
		var enemyInstance = (UFOCharacterBody2D) enemy.Instantiate();
		enemyInstance.Position = location;
		enemyInstance.Connect(UFOCharacterBody2D.SignalName.Shoot, new Callable(this, MethodName._on_ufo_shoot));
		AddChild (enemyInstance);
	}
}
