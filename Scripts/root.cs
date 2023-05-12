using Godot;
using System;

public partial class root : Node2D
{
	public void _on_player_shoot(PackedScene laser, float direction, Vector2 location)
	{
		var laserInstance = (Laser)laser.Instantiate();
		laserInstance.Rotation = direction;
		laserInstance.Position = location + Position;
		AddChild(laserInstance);
	}
}	
