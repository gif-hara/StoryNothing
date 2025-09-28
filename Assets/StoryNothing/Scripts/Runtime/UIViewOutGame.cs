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

        private readonly HKUIDocument skillBoardArea;

        private readonly RectTransform skillBoardBackground;

        private readonly GameObject skillBoardBlackout;

        private readonly HKUIDocument cellPrefab;

        private readonly HKUIDocument skillPiecePrefab;

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
            skillBoardArea = this.document
                .Q<HKUIDocument>("Area.Center")
                .Q<HKUIDocument>("Area.SkillBoard");
            skillBoardBackground = skillBoardArea
                .Q<RectTransform>("Background");
            skillBoardBlackout = skillBoardArea
                .Q("Blackout");
            skillBoardBlackout.SetActive(false);
            cellPrefab = this.document
                .Q<HKUIDocument>("UI.Element.Cell");
            skillPiecePrefab = this.document
                .Q<HKUIDocument>("UI.Element.SkillPiece");
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
            var uiElementSkillPiece = new UIElementSkillPiece(Object.Instantiate(skillPiecePrefab, skillBoardArea.transform));
            while (!cancellationToken.IsCancellationRequested)
            {
                var selectEditModeScope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                uiElementSkillPiece.SetPositionInCenter();
                skillBoardBlackout.SetActive(true);
                var selectEditModeResult = await UniTask.WhenAny(
                    UniTask.WhenAny(
                        userData.SkillPieces.Select(x =>
                            CreateHKButton(CreateListContent(x.Value.Name, selectEditModeScope.Token))
                                .OnPointerEnter(_ => uiElementSkillPiece.Setup(x.Value, 0))
                                .OnClickAsync(selectEditModeScope.Token)
                    )),
                    playerInput.actions["UI/Cancel"].OnPerformedAsObservable().FirstAsync(selectEditModeScope.Token).AsUniTask()
                );
                selectEditModeScope.Cancel();
                selectEditModeScope.Dispose();
                if (selectEditModeResult.winArgumentIndex == 0)
                {
                    var skillPiecePlacementScope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    var skillPiece = userData.SkillPieces[selectEditModeResult.result1];
                    skillBoardBlackout.SetActive(false);
                    var rotationIndex = 0;
                    var skillPieceSize = skillPiece.SkillPieceCellSpec.GetSize(rotationIndex);
                    Mouse.current.WarpCursorPosition(uiElementSkillPiece.WorldToScreenPoint());
                    while (!skillPiecePlacementScope.IsCancellationRequested)
                    {
                        await UniTask.Yield(PlayerLoopTiming.Update, skillPiecePlacementScope.Token);
                        var scrollValue = playerInput.actions["UI/ScrollWheel"].ReadValue<Vector2>().y;
                        if (scrollValue > 0.1f)
                        {
                            rotationIndex = (rotationIndex + 1) % 4;
                            skillPieceSize = skillPiece.SkillPieceCellSpec.GetSize(rotationIndex);
                            uiElementSkillPiece.Setup(skillPiece, rotationIndex);
                        }
                        else if (scrollValue < -0.1f)
                        {
                            rotationIndex = (rotationIndex + 3) % 4;
                            skillPieceSize = skillPiece.SkillPieceCellSpec.GetSize(rotationIndex);
                            uiElementSkillPiece.Setup(skillPiece, rotationIndex);
                        }
                        var skillBoard = userData.GetEquipInstanceSkillBoard();
                        uiElementSkillPiece.SetPositionFromMouse(new Vector2(0.0f, 0.0f), skillBoard.SkillBoardSpec.Size, skillPieceSize);
                        if (playerInput.actions["UI/Submit"].WasPerformedThisFrame())
                        {
                            Debug.Log($"スキルピース配置: {uiElementSkillPiece.GetPositionIndexFromMousePosition(skillBoard.SkillBoardSpec.Size, skillPieceSize)}");
                            break;
                        }
                        if (playerInput.actions["UI/Cancel"].WasPerformedThisFrame())
                        {
                            Debug.Log("スキルピース配置キャンセル");
                            break;
                        }
                    }
                    skillPiecePlacementScope.Cancel();
                    skillPiecePlacementScope.Dispose();
                }
                else if (selectEditModeResult.winArgumentIndex == 1)
                {
                    Debug.Log("スキルボード編集キャンセル");
                    break;
                }
            }
            uiElementSkillPiece.Dispose();
            skillBoardBlackout.SetActive(false);
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
