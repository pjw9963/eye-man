using Godot;
using System;
using System.Threading.Tasks;

public partial class root : Node2D
{
    public async void _on_player_shoot(PackedScene laser, float direction, Vector2 location)
    {
        // spawn laser and set rotation and direction
        var laserInstance = (Laser)laser.Instantiate();
        laserInstance.Rotation = direction;
        laserInstance.Position = location;
        AddChild(laserInstance);
    }
}
