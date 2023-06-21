using System;
using System.Threading.Tasks;
using Godot;

public partial class Missle : CharacterBody2D
{
	public const float Speed = 100.0f;

	public int launchDelay = 1000;

	private PackedScene
		_explosion = GD.Load<PackedScene>("res://Scenes/Explosion.tscn");

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
		if (!IsInstanceValid(targetEnemy)) {
			var targetAquired = setTarget();
			
			if (!targetAquired) {
				MoveAndSlide();
				return;
			}
		}

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

			Velocity += velocity;
		}
		else
		{
			// idk what this is supposed to do
			// case when no direction is set, AKA not launched
			// probably just supposed to be deceleration
			// velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			// velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		MoveAndSlide();
	}

	private void _on_area_2d_area_entered(Area2D area)
	{
		// not gonna work, exp instance is attached to missile, which is getting QueueFreed
		// need to signal up so exp instance is apart of root node
		var expInstance = (Explosion) _explosion.Instantiate();
		expInstance.Position = Position;

		GetTree()
			.Root
			.GetChild(0)
			.CallDeferred(Node.MethodName.AddChild, expInstance);

		this.QueueFree();
		// spawn explosion at location
		// EmitSignal(SignalName.Destroyed, 20); // worth 20 points
	}
}
