using System;
using System.Threading.Tasks;
using Godot;

public partial class Missle : CharacterBody2D
{
	public const float Speed = 1.0f;

	public int launchDelay = 1000;
	public Vector2 initDestination;

	private Node2D targetEnemy;

	private Vector2 direction;

	private bool LAUNCHED = false;

	public override void _Ready()
	{
		MotionMode = MotionModeEnum.Floating;
		base._Ready();
		bool targetSet = setTarget();
		lauch();
	}

	private async Task lauch()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(launchDelay));
		LAUNCHED = true;
	}

	// sets targetEnemy to closest node in enemies group
	// returns false if no enemy exists
	private bool setTarget()
	{
		Godot.Collections.Array<Node> enemies =
			GetTree().GetNodesInGroup("enemies");

		if (enemies.Count == 0) return false;

		float currentDistance;
		float minDistance = float.MaxValue;

		foreach (Node2D enemy in enemies)
		{
			currentDistance = Position.DistanceTo(enemy.Position);
			if (currentDistance < minDistance)
			{
				minDistance = currentDistance;
				targetEnemy = enemy;
			}
		}

		return true;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (LAUNCHED)
		{
			direction = (targetEnemy.Position - this.Position).Normalized();
		}

		if (!LAUNCHED)
		{
		}

		Vector2 velocity = Velocity;

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity += velocity;
		MoveAndSlide();
	}
}
