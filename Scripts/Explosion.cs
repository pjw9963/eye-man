using System;
using System.Threading.Tasks;
using Godot;

public partial class Explosion : Area2D
{
	private Sprite2D _sprite;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		_sprite = (Sprite2D) GetNode<Node2D>("Sprite2D");

		await Task.Delay(TimeSpan.FromMilliseconds(20));
		var areasInRadius = GetOverlappingAreas();
		GD.Print(areasInRadius.Count);
		foreach (var item in areasInRadius)
		{
			item.GetParent().CallDeferred(Node.MethodName.QueueFree);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_sprite.Modulate.A == 0)
		{
			this.QueueFree();
			return;
		}

		var mod = _sprite.Modulate;
		mod.A -= 0.01F;
		_sprite.Modulate = mod;
	}
}
