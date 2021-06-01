using Godot;
using System;
using System.Collections.Generic;

namespace GrowYourMoss.src
{
    public class GrowPoint : Position2D
    {
        static private Random _random = new Random();

        // 発生確率補正値
        [Export]
        public float reviseAppear = 1;
        // 湿度
        [Export]
        public float humidity = 1;
        // 光量
        [Export]
        public int light = 0;

        private static PackedScene[] mossScenes = { GD.Load<PackedScene>("res://src/BartramiaPomiformis.tscn"),
                                              GD.Load<PackedScene>("res://src/Fissidens.tscn"),
                                              GD.Load<PackedScene>("res://src/Leucobryaceae.tscn"),
                                              GD.Load<PackedScene>("res://src/LeucobryumScabrum.tscn"),
                                              GD.Load<PackedScene>("res://src/Dicranales.tscn")};

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        public void Grow(int areaAir, int areaHumidity, int areaLight)
        {
        }

        public void Appear(int areaHumidity, int areaLight)
        {
            var canAppearMoss = new List<PackedScene>();

            foreach (var mossScene in mossScenes)
            {
                Moss moss = mossScene.Instance() as Moss;

                if (moss.CanGrow(areaHumidity * humidity, areaLight + light))
                {
                    canAppearMoss.Add(mossScene);
                }
            }

            if (canAppearMoss.Count == 0)
            {
                return;
            }

            AddChild(canAppearMoss[_random.Next(canAppearMoss.Count)].Instance());
        }

        // 苔が生えているかどうか
        public bool IsAppear()
        {
            return GetChildCount() == 0 ? false : true;
        }

        public bool RollAppear(int areaAir)
        {
            int roll = ((-(areaAir * areaAir) + 25) / 5) + 1;
            int rand = _random.Next(1, 101);

            return rand <= roll * reviseAppear;
        }

        public void PlayAnim()
        {
            var moss = GetChild<Moss>(0);
            moss.PlayAnim();
        }
    }
}
