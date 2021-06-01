using Godot;

public interface Moss
{
    bool CanGrow(float humidity, int light);
    void Grow();
    void PlayAnim();
}
