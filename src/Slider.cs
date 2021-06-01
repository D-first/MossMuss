using Godot;
using System;

public class Slider : Control
{
    [Signal]
    delegate void Changed(float value);

    private bool mouseInSlider = false;
    private TextureProgress slider;

    public override void _Ready()
    {
        slider = GetNode<TextureProgress>("TextureProgress");
    }

    public override void _Input(InputEvent @event)
    {
        if (mouseInSlider && Input.IsMouseButtonPressed((int)ButtonList.Left))
        {
            SetValue(GetNode<TextureProgress>("TextureProgress"));
            EmitSignal("Changed", slider.Value);
        }
    }

    private void SetValue(TextureProgress slider)
    {
        slider.Value = RatioInBody(slider) * slider.MaxValue;
    }

    public float RatioInBody(TextureProgress slider)
    {
        var posClicked = GetLocalMousePosition() - slider.RectPosition;
        float ratio = posClicked.x / slider.RectSize.x;

        if (ratio > 1.0)
        {
            ratio = 1.0f;
        }
        else if (ratio < 0.0)
        {
            ratio = 0.0f;
        }

        return ratio;
    }

    private void _on_TextureProgress_mouse_entered()
    {
        mouseInSlider = true;
    }


    private void _on_TextureProgress_mouse_exited()
    {
        mouseInSlider = false;
    }
}



