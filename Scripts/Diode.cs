using Godot;
using System;

public partial class Diode : Node2D
{

    public override void _Ready()
    {
        base._Ready();

    }

    public void _on_player_shoot(PackedScene laser, float direction, Vector2 location)
    {
        var audioSteam = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        audioSteam.Play();
    }
}
