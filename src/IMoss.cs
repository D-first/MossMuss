using Godot;

public interface IMoss
{
    bool CanGrow(float humidity, int light);
    void Grow();
    void PlayAnim();
}
