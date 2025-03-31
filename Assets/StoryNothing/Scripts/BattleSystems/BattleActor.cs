using UnityEngine;

namespace StoryNothing.BattleSystems
{
    public class BattleActor
    {
        public string Name { get; set; }

        public int HitPoint { get; set; }

        public BattleActor(string name, int hitPoint)
        {
            Name = name;
            HitPoint = hitPoint;
        }
    }
}
