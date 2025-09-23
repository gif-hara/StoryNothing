using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StoryNothing.UIViews
{
    public sealed class UIViewOutGame
    {
        private readonly HKUIDocument document;

        private readonly UserData userData;

        private readonly Transform listParent;

        private readonly HKUIDocument listContentPrefab;

        public UIViewOutGame(HKUIDocument document, UserData userData)
        {
            this.document = document;
            this.userData = userData;
            listParent = this.document
                .Q<HKUIDocument>("Area.Left")
                .Q<HKUIDocument>("List")
                .Q<Transform>("Content");
            listContentPrefab = this.document
                .Q<HKUIDocument>("UI.Element.Button");
        }

        public async UniTask BeginAsync(CancellationToken cancellationToken)
        {
            await StateRootAsync(cancellationToken);
        }

        private async UniTask StateRootAsync(CancellationToken cancellationToken)
        {
            document.gameObject.SetActive(true);
            while (!cancellationToken.IsCancellationRequested)
            {
                var scope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                var result = await UniTask.WhenAny(
                    GetHKButton(CreateListContent("スキルボード変更", scope.Token))
                        .OnClickAsync(cancellationToken),
                    GetHKButton(CreateListContent("闘技場へ", scope.Token))
                        .OnClickAsync(cancellationToken)
                );
                scope.Cancel();
                scope.Dispose();
                switch (result)
                {
                    case 0:
                        await StateChangeSkillBoardAsync(cancellationToken);
                        break;
                    case 1:
                        await StateSelectBattleAsync(cancellationToken);
                        break;
                }
            }
        }

        private async UniTask StateChangeSkillBoardAsync(CancellationToken cancellationToken)
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var result = await UniTask.WhenAny(
                userData.SkillBoards.Select(x => GetHKButton(CreateListContent(x.Name, scope.Token)).OnClickAsync(cancellationToken))
            );
            var selectedSkillBoard = userData.SkillBoards[result];
            userData.EquipInstanceSkillBoardId = selectedSkillBoard.InstanceId;
            scope.Cancel();
            scope.Dispose();
        }

        private UniTask StateSelectBattleAsync(CancellationToken cancellationToken)
        {
            return UniTask.Never(cancellationToken);
        }

        private HKUIDocument CreateListContent(string text, CancellationToken cancellationToken)
        {
            var instance = Object.Instantiate(listContentPrefab, listParent);
            instance.Q<TMP_Text>("Text").SetText(text);
            cancellationToken.RegisterWithoutCaptureExecutionContext(() => Object.Destroy(instance.gameObject));
            return instance;
        }

        private static HKButton GetHKButton(HKUIDocument document)
        {
            return new HKButton(document.Q<Button>("Button"));
        }
    }
}
