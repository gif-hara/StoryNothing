using StoryNothing.BattleSystems;
using UnityEngine;

namespace StoryNothing
{
    public interface IBattleSetupData
    {
        Party PlayerParty { get; }

        Party EnemyParty { get; }
    }
}
