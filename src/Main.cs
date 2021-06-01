using Godot;
using GrowYourMoss.src;
using System;

public class Main : Node2D
{
    static private Random _random = new Random();

    // 最大ターン数
    public int MaxTurn = 100;

    // 現在の経過ターン数
    public int turn = 0;
    // 風通し
    [Export(PropertyHint.Range, "-5,5,1")]
    public int air = 0;
    // 湿度
    [Export(PropertyHint.Range, "-10,10,1")]
    public int humidity = 0;
    // 光量
    [Export(PropertyHint.Range, "-10,10,1")]
    public int light = 0;

    // 瓶の蓋が閉まっているか
    private bool lidClosed = true;

    public Node2D growPoints;
    private Light2D light2D;

    public override void _Ready()
    {
        growPoints = GetNode<Node2D>("GrowPoints");
        light2D = GetNode<Light2D>("UI/Light2D");
        GetNode<TextureButton>("Result/Return").Visible = false;
    }
    public void Next()
    {
        // ターンを1すすめる
        turn += 1;

        // 湿度低下イベント
        DownHumidity();
        // 風通し変化イベント
        ChangeAir();
        // 苔発生・成長イベント
        Grow();

        GetNode<Label>("UI/Turn").Text = turn.ToString();

        // ゲーム終了
        if (IsEndGame())
        {
            GetNode<Control>("UI").Visible = false;

            foreach (GrowPoint growPoint in growPoints.GetChildren())
            {
                if (growPoint.IsAppear())
                {
                    growPoint.PlayAnim();
                }
            }
        }
    }

    private void DownHumidity()
    {
        if (!lidClosed)
        {
            humidity -= 1;
        }
        humidity -= _random.Next(2);
        ChangeAreaStatus();
    }

    private void ChangeAir()
    {
        int[] table = { 0, 0, 1 };
        var rand = table[_random.Next(table.Length)];
        var value = lidClosed ? -rand : rand;

        air += value;
        ChangeAreaStatus();
    }

    private void Grow()
    {
        foreach (GrowPoint growPoint in growPoints.GetChildren())
        {
            if (growPoint.IsAppear())
            {
                growPoint.Grow(air, humidity, light);
            }
            else if (growPoint.RollAppear(air))
            {
                growPoint.Appear(humidity, light);
            }
        }
    }

    private void _on_Open_pressed()
    {
        if (!lidClosed)
        {
            return;
        }

        lidClosed = false;
        GetNode<AudioStreamPlayer>("OpenCork").Play();
        GetNode<TextureRect>("Cork").Visible = false;
    }


    private void _on_Close_pressed()
    {
        if (lidClosed)
        {
            return;
        }

        lidClosed = true;
        GetNode<AudioStreamPlayer>("CloseCork").Play();
        GetNode<TextureRect>("Cork").Visible = true;
    }


    private void _on_UpLight_pressed()
    {
        light += _random.Next(0, 2) + 1;
    }


    private void _on_DownLight_pressed()
    {
        light += _random.Next(-1, 1) - 1;
    }


    private void _on_UpHumidity_pressed()
    {
        GetNode<TextureRect>("Cork").Visible = false;
        GetNode<AudioStreamPlayer>("Spray").Play();
        humidity += 3;
        ChangeAreaStatus();
    }
    private void _on_Next_pressed()
    {
        GetNode<AudioStreamPlayer>("NextDay").Play();
        Next();
    }

    private void ChangeAreaStatus()
    {
        air = Math.Min(5, Math.Max(air, -5));
        humidity = Math.Min(10, Math.Max(humidity, -10));
        light = Math.Min(10, Math.Max(light, -10));

        if (5 <= humidity)
        {
            GetNode<Sprite>("FrontSoil").Texture = GD.Load<Texture>("res://assets/front_soil_3.png");
            GetNode<Sprite>("BackSoil").Texture = GD.Load<Texture>("res://assets/back_soil_3.png");
            GetNode<Sprite>("Stairs").Texture = GD.Load<Texture>("res://assets/stairs_2.png");
        }
        else if (0 <= humidity)
        {
            GetNode<Sprite>("FrontSoil").Texture = GD.Load<Texture>("res://assets/front_soil_2.png");
            GetNode<Sprite>("BackSoil").Texture = GD.Load<Texture>("res://assets/back_soil_2.png");
        }
        else if (-5 <= humidity)
        {
            GetNode<Sprite>("FrontSoil").Texture = GD.Load<Texture>("res://assets/front_soil_1.png");
            GetNode<Sprite>("BackSoil").Texture = GD.Load<Texture>("res://assets/back_soil_1.png");
            GetNode<Sprite>("Stairs").Texture = GD.Load<Texture>("res://assets/stairs_1.png");
        }
        else if (-10 <= humidity)
        {
            GetNode<Sprite>("FrontSoil").Texture = GD.Load<Texture>("res://assets/front_soil_0.png");
            GetNode<Sprite>("BackSoil").Texture = GD.Load<Texture>("res://assets/back_soil_0.png");
        }

        //デバッグ表示
        GetNode<Label>("UI/Panel/DebugAirValue").Text = air.ToString();
        GetNode<Label>("UI/Panel/DebugHumidityValue").Text = humidity.ToString();
        GetNode<Label>("UI/Panel/DebugLightValue").Text = light.ToString();
        GetNode<Label>("UI/Panel/DebugTurnValue").Text = turn.ToString();
    }
    private void _on_Spray_finished()
    {
        if (lidClosed)
        {
            GetNode<TextureRect>("Cork").Visible = true;
        }
    }
    private void _on_Slider_Changed(float value)
    {
        light2D.Energy = value * 0.05f;
        light = (int)value - 10;
        GetNode<Label>("UI/Panel/DebugLightValue").Text = light.ToString();
    }

    private bool IsEndGame()
    {
        return MaxTurn <= turn;
    }

    public override void _Input(InputEvent _event)
    {
        if (!IsEndGame())
        {
            return;
        }
        if (_event is InputEventMouseButton)
        {
            if (_event.IsPressed())
            {
                GetNode<TextureButton>("Result/Return").Visible = true; ;
            }
        }
    }
    private void _on_Return_pressed()
    {
        GetTree().ChangeScene("res://src/Title.tscn");
    }

    private void _on_Help_pressed()
    {
        GetNode<TextureButton>("HowTo/CanvasLayer/HowToWindow").Visible = true;
    }
    private void _on_HowToWindow_pressed()
    {
        GetNode<TextureButton>("HowTo/CanvasLayer/HowToWindow").Visible = false;
    }
}
