using Godot;
using System;

public class Moss : Node2D
{
    [Export(PropertyHint.Range, "-10,10,1")]
    private int maxHumidity;
    [Export(PropertyHint.Range, "-10,10,1")]
    private int minHumidity;
    [Export(PropertyHint.Range, "-10,10,1")]
    private int maxLight;
    [Export(PropertyHint.Range, "-10,10,1")]
    private int minLight;

    public override void _Ready()
    {
        RotationDegrees = new Random().Next(-20, 21);
    }

    public bool CanGrow(float humidity, int light)
    {
        bool humidityRangeIn = false;
        if (minHumidity <= humidity && humidity <= maxHumidity)
        {
            humidityRangeIn = true;
        }

        bool lightRangeIn = false;
        if (minLight <= light && light <= maxLight)
        {
            lightRangeIn = true;
        }

        return humidityRangeIn && lightRangeIn;
    }

    public void Grow()
    {
    }

    public void PlayAnim()
    {
        GetNode<AnimatedSprite>("AnimatedSprite").Play();
    }
}
