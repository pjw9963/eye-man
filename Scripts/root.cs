using System;
using System.Threading.Tasks;
using Godot;

public partial class root : Node2D
{
	private int playerScore = 0;

	public async void _on_player_shoot(Node projectile)
	{
		AddChild (projectile);
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
		enemyInstance.AddToGroup("enemies");
		enemyInstance.Position = location;

		// wire-up signals
		enemyInstance
			.Connect(UFOCharacterBody2D.SignalName.Shoot,
			new Callable(this, MethodName._on_ufo_shoot));
		enemyInstance
			.Connect(UFOCharacterBody2D.SignalName.Destroyed,
			new Callable(this, MethodName.updateScore));

		AddChild (enemyInstance);
	}

	private void updateScore(int points)
	{
		playerScore += points;

		var labelNode = GetNode<Label>("Label");

		labelNode.Text = $"Score: {playerScore}";
	}
}
