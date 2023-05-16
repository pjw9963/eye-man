using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : CharacterBody2D
{
    [Export]
    public int Speed { get; set; } = 400;
    [Signal]
    public delegate void ShootEventHandler(PackedScene laser, float direction, Vector2 location);
    private PackedScene _laser = GD.Load<PackedScene>("res://Scenes/Laser.tscn");
    private Node2D _diodeNode;

    public const int SHOOT_COOLDOWN = 250;
    private bool canShoot;


    public override void _Ready()
    {
        base._Ready();
        canShoot = true;
        _diodeNode = GetNode<Node2D>("Diode");
    }

    public void GetInput()
    {

        Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
        Velocity = inputDirection * Speed;
        if (Input.IsActionPressed("Left") ||
            Input.IsActionPressed("Right") ||
            Input.IsActionPressed("Up") ||
            Input.IsActionPressed("Down"))
            Rotation = inputDirection.Angle();
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
    }

    public async override void _Process(double delta)
    {
        if (Input.IsActionPressed("Fire") && canShoot)
        {
            EmitSignal(SignalName.Shoot, _laser, Rotation, _diodeNode.GlobalPosition);

            // cooldown logic
            canShoot = false;
            await Task.Delay(TimeSpan.FromMilliseconds(SHOOT_COOLDOWN));
            canShoot = true;
        }
    }
}
