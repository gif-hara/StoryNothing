using UnityEngine;

namespace StoryNothing
{
    [CreateAssetMenu(fileName = "GameRules", menuName = "StoryNothing/GameRules")]
    public class GameRules : ScriptableObject
    {
        [SerializeField]
        private float playerMoveSpeed;
        public float PlayerMoveSpeed => playerMoveSpeed;

        [SerializeField]
        private float playerRotationSpeed;
        public float PlayerRotationSpeed => playerRotationSpeed;
    }
}
