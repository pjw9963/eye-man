using Godot;
using System;

public partial class Laser : Node2D
{
    private Area2D _laserArea;
    public int SPEED = 30;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _laserArea = GetNode<Area2D>("LaserArea");
        // _laserArea.Position = Position;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var vosn = GetNode<VisibleOnScreenNotifier2D>("LaserArea/LaserOnScreenNotifier2D");

    }

    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(MathF.Cos(Rotation), MathF.Sin(Rotation)) * SPEED;
    }

    public void _on_laser_on_screen_notifier_2d_screen_exited()
    {
        this.QueueFree();
    }
}
