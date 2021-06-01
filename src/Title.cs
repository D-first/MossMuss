using Godot;
using GrowYourMoss.src;

public class Title : Node2D
{
    public override void _Ready()
    {

    }

    public override void _Input(InputEvent _event)
    {
        if (_event is InputEventMouseButton)
        {
            if (_event.IsPressed())
            {
                GetNode<AnimationPlayer>("AnimationPlayer").Play("start");
                GetNode<AudioStreamPlayer>("StartSE").Play();
            }
        }
    }
    private void _on_StartSE_finished()
    {
        GetTree().ChangeScene("res://src/Main.tscn");
    }
}
