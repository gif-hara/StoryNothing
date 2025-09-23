using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using HK;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StoryNothing.UIViews
{
    public sealed class UIViewOutGame
    {
        private readonly HKUIDocument document;

        private readonly Transform listParent;

        private readonly HKUIDocument listContentPrefab;

        public UIViewOutGame(HKUIDocument document)
        {
            this.document = document;
            listParent = this.document
                .Q<HKUIDocument>("Area.Left")
                .Q<HKUIDocument>("List")
                .Q<Transform>("Content");
            listContentPrefab = this.document
                .Q<HKUIDocument>("UI.Element.Button");
        }

        public async Task<UniTask> BeginAsync(CancellationToken cancellationToken)
        {
            await StateRootAsync(cancellationToken);
            return UniTask.CompletedTask;
        }

        private UniTask StateRootAsync(CancellationToken cancellationToken)
        {
            document.gameObject.SetActive(true);
            var changeSkillBoardContent = CreateListContent("スキルボード変更");
            changeSkillBoardContent.Q<Button>("Button").OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Debug.Log("TODO: Change SkillBoard");
                })
                .RegisterTo(cancellationToken);
            var selectBattleContent = CreateListContent("闘技場へ");
            selectBattleContent.Q<Button>("Button").OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Debug.Log("TODO: Select Battle");
                })
                .RegisterTo(cancellationToken);
            return UniTask.Never(cancellationToken);
        }

        private HKUIDocument CreateListContent(string text)
        {
            var instance = UnityEngine.Object.Instantiate(listContentPrefab, listParent);
            instance.Q<TMP_Text>("Text").SetText(text);
            return instance;
        }
    }
}
