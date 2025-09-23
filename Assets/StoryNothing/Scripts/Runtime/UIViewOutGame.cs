using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using StoryNothing.InstanceData;
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

        private InstanceSkillBoard selectedInstanceSkillBoard;

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
                    GetHKButton(CreateListContent("スキルボード編集", scope.Token))
                        .OnClickAsync(cancellationToken),
                    GetHKButton(CreateListContent("闘技場へ", scope.Token))
                        .OnClickAsync(cancellationToken)
                );
                scope.Cancel();
                scope.Dispose();
                switch (result)
                {
                    case 0:
                        await StateSelectEquipSkillBoardAsync(cancellationToken);
                        break;
                    case 1:
                        await StateSelectEditSkillBoardAsync(cancellationToken);
                        break;
                    case 2:
                        await StateSelectBattleAsync(cancellationToken);
                        break;
                }
            }
        }

        private async UniTask StateSelectEquipSkillBoardAsync(CancellationToken cancellationToken)
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var result = await UniTask.WhenAny(
                userData.SkillBoards.Select(x => GetHKButton(CreateListContent(x.Name, scope.Token)).OnClickAsync(cancellationToken))
            );
            userData.EquipInstanceSkillBoardId = userData.SkillBoards[result].InstanceId;
            scope.Cancel();
            scope.Dispose();
        }

        private async UniTask StateSelectEditSkillBoardAsync(CancellationToken cancellationToken)
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var result = await UniTask.WhenAny(
                userData.SkillBoards.Select(x => GetHKButton(CreateListContent(x.Name, scope.Token)).OnClickAsync(cancellationToken))
            );
            selectedInstanceSkillBoard = userData.SkillBoards[result];
            scope.Cancel();
            scope.Dispose();
            await StateEditSkillBoardAsync(cancellationToken);
        }

        private UniTask StateEditSkillBoardAsync(CancellationToken cancellationToken)
        {
            return UniTask.Never(cancellationToken);
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
