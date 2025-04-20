using HK;
using UnityEngine;

namespace StoryNothing
{
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField]
        private GameRules gameRules;

        [SerializeField]
        private HKUIDocument backgroundDocument;

        private void Start()
        {
            ServiceLocator.Register(gameRules, destroyCancellationToken);
            ServiceLocator.Register(new InputController(), destroyCancellationToken);
            ServiceLocator.Register(new UserData(), destroyCancellationToken);

            var uiViewBackground = new UIViewBackground(backgroundDocument);
            uiViewBackground.Setup(destroyCancellationToken);
            uiViewBackground.Open();
        }
    }
}
