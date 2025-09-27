using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using R3;
using StoryNothing.InstanceData;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace StoryNothing.UIViews
{
    public sealed class UIViewOutGame
    {
        private readonly HKUIDocument document;

        private readonly UserData userData;

        private readonly PlayerInput playerInput;

        private readonly Transform listParent;

        private readonly HKUIDocument listContentPrefab;

        private readonly RectTransform skillBoardBackground;

        private readonly HKUIDocument cellPrefab;

        private readonly List<UIElementCell> holeElements = new();

        public UIViewOutGame(HKUIDocument document, UserData userData, PlayerInput playerInput)
        {
            this.document = document;
            this.userData = userData;
            this.playerInput = playerInput;
            listParent = this.document
                .Q<HKUIDocument>("Area.Left")
                .Q<HKUIDocument>("List")
                .Q<Transform>("Content");
            listContentPrefab = this.document
                .Q<HKUIDocument>("UI.Element.Button");
            skillBoardBackground = this.document
                .Q<HKUIDocument>("Area.Center")
                .Q<HKUIDocument>("Area.SkillBoard")
                .Q<RectTransform>("Background");
            cellPrefab = this.document
                .Q<HKUIDocument>("UI.Element.Cell");
        }

        public async UniTask BeginAsync(CancellationToken cancellationToken)
        {
            await StateRootAsync(cancellationToken);
        }

        private async UniTask StateRootAsync(CancellationToken cancellationToken)
        {
            document.gameObject.SetActive(true);
            SetSkillBoard(userData.GetEquipInstanceSkillBoard());
            while (!cancellationToken.IsCancellationRequested)
            {
                var scope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                var result = await UniTask.WhenAny(
                    CreateHKButton(CreateListContent("スキルボード変更", scope.Token))
                        .OnClickAsync(cancellationToken),
                    CreateHKButton(CreateListContent("スキルボード編集", scope.Token))
                        .OnClickAsync(cancellationToken),
                    CreateHKButton(CreateListContent("闘技場へ", scope.Token))
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
                        await StateEditSkillBoardAsync(cancellationToken);
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
                UniTask.WhenAny(
                    userData.SkillBoards.Select(x =>
                        CreateHKButton(CreateListContent(x.Value.Name, scope.Token))
                            .OnPointerEnter(hkButton => SetSkillBoard(x.Value))
                            .OnClickAsync(cancellationToken)
                )),
                playerInput.actions["UI/Cancel"].OnPerformedAsObservable().FirstAsync(cancellationToken).AsUniTask()
            );
            if (result.winArgumentIndex == 0)
            {
                userData.EquipInstanceSkillBoardId = userData.SkillBoards[result.result1].InstanceId;
            }
            else if (result.winArgumentIndex == 1)
            {
                SetSkillBoard(userData.GetEquipInstanceSkillBoard());
            }
            scope.Cancel();
            scope.Dispose();
        }

        private async UniTask StateEditSkillBoardAsync(CancellationToken cancellationToken)
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var result = await UniTask.WhenAny(
                UniTask.WhenAny(
                    userData.SkillPieces.Select(x =>
                        CreateHKButton(CreateListContent(x.Value.Name, scope.Token))
                            .OnClickAsync(cancellationToken)
                )),
                playerInput.actions["UI/Cancel"].OnPerformedAsObservable().FirstAsync(cancellationToken).AsUniTask()
            );
            if (result.winArgumentIndex == 0)
            {
                Debug.Log("選択されたスキルピース: " + userData.SkillPieces[result.result1].Name);
            }
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
            cancellationToken.RegisterWithoutCaptureExecutionContext(() =>
            {
                if (instance != null)
                {
                    Object.Destroy(instance.gameObject);
                }
            });
            return instance;
        }

        private void SetSkillBoard(InstanceSkillBoard instanceSkillBoard)
        {
            foreach (var element in holeElements)
            {
                element.Dispose();
            }
            holeElements.Clear();
            skillBoardBackground.sizeDelta = new Vector2(
                instanceSkillBoard.SkillBoardSpec.X * 100,
                instanceSkillBoard.SkillBoardSpec.Y * 100
                );
            foreach (var hole in instanceSkillBoard.Holes)
            {
                var instance = new UIElementCell(Object.Instantiate(cellPrefab, skillBoardBackground));
                instance.SetPosition(hole, instanceSkillBoard.SkillBoardSpec.X, instanceSkillBoard.SkillBoardSpec.Y);
                instance.SetBackgroundColor(Define.SkillPieceColor.Gray);
                holeElements.Add(instance);
            }
        }

        private static HKButton CreateHKButton(HKUIDocument document)
        {
            return new HKButton(document.Q<Button>("Button"));
        }
    }
}
