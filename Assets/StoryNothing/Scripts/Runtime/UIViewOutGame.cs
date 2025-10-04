using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly int playerCharacterSpecId;

        private readonly Transform listParent;

        private readonly HKUIDocument listContentPrefab;

        private readonly HKUIDocument skillBoardArea;

        private readonly RectTransform skillBoardBackground;

        private readonly GameObject skillBoardBlackout;

        private readonly RectTransform newSkillPieceParent;

        private readonly HKUIDocument cellPrefab;

        private readonly HKUIDocument skillPiecePrefab;

        private readonly List<UIElementCell> holeElements = new();

        private readonly List<UIElementSkillPiece> skillPieceElements = new();

        private readonly TMP_Text hitPointLabel;

        private readonly TMP_Text physicalAttackLabel;

        private readonly TMP_Text physicalDefenseLabel;

        private readonly TMP_Text magicalAttackLabel;

        private readonly TMP_Text magicalDefenseLabel;

        private readonly TMP_Text speedLabel;

        public UIViewOutGame(HKUIDocument document, UserData userData, PlayerInput playerInput, int playerCharacterSpecId)
        {
            this.document = document;
            this.userData = userData;
            this.playerInput = playerInput;
            this.playerCharacterSpecId = playerCharacterSpecId;
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
            newSkillPieceParent = this.document
                .Q<HKUIDocument>("Area.Center")
                .Q<RectTransform>("NewSkillPieceParent");
            cellPrefab = this.document
                .Q<HKUIDocument>("UI.Element.Cell");
            skillPiecePrefab = this.document
                .Q<HKUIDocument>("UI.Element.SkillPiece");
            hitPointLabel = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<HKUIDocument>("UI.Element.Parameter.HitPoint")
                .Q<TMP_Text>("Value");
            physicalAttackLabel = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<HKUIDocument>("UI.Element.Parameter.PhysicalAttack")
                .Q<TMP_Text>("Value");
            physicalDefenseLabel = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<HKUIDocument>("UI.Element.Parameter.PhysicalDefense")
                .Q<TMP_Text>("Value");
            magicalAttackLabel = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<HKUIDocument>("UI.Element.Parameter.MagicalAttack")
                .Q<TMP_Text>("Value");
            magicalDefenseLabel = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<HKUIDocument>("UI.Element.Parameter.MagicalDefense")
                .Q<TMP_Text>("Value");
            speedLabel = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<HKUIDocument>("UI.Element.Parameter.Speed")
                .Q<TMP_Text>("Value");
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
            var uiElementSkillPiece = new UIElementSkillPiece(UnityEngine.Object.Instantiate(skillPiecePrefab, newSkillPieceParent));
            while (!cancellationToken.IsCancellationRequested)
            {
                var selectEditModeScope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                skillBoardBlackout.SetActive(true);
                var availableSkillPieces = userData.SkillPieces
                    .Where(x => !userData.GetEquipInstanceSkillBoard().PlacementSkillPieces.Any(y => y.InstanceSkillPieceId == x.Value.InstanceId))
                    .Select(x => x.Value)
                    .ToList();
                var selectEditModeResult = await UniTask.WhenAny(
                    UniTask.WhenAny(
                        availableSkillPieces.Select(x =>
                            CreateHKButton(CreateListContent(x.Name, selectEditModeScope.Token))
                                .OnPointerEnter(_ =>
                                {
                                    uiElementSkillPiece.Setup(x, 0);
                                    uiElementSkillPiece.SetPositionInCenter();
                                })
                                .OnClickAsync(selectEditModeScope.Token)
                    )),
                    GetClickedUIElementSkillPieceAsync(uiElementSkillPiece, selectEditModeScope.Token),
                    playerInput.actions["UI/Cancel"].OnPerformedAsObservable().FirstAsync(selectEditModeScope.Token).AsUniTask()
                );
                selectEditModeScope.Cancel();
                selectEditModeScope.Dispose();
                if (selectEditModeResult.winArgumentIndex == 0)
                {
                    var skillPiecePlacementScope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    var skillPiece = availableSkillPieces[selectEditModeResult.result1];
                    skillBoardBlackout.SetActive(false);
                    var rotationIndex = 0;
                    Mouse.current.WarpCursorPosition(uiElementSkillPiece.WorldToScreenPoint());
                    await BeginSkillPiecePlacementAsync(
                        rotationIndex,
                        skillPiece.SkillPieceCellSpec.GetSize(rotationIndex),
                        skillPiece,
                        uiElementSkillPiece,
                        skillPiecePlacementScope.Token
                        );
                    skillPiecePlacementScope.Cancel();
                    skillPiecePlacementScope.Dispose();
                }
                else if (selectEditModeResult.winArgumentIndex == 1)
                {
                    uiElementSkillPiece.Dispose();
                    uiElementSkillPiece = selectEditModeResult.result2;
                    uiElementSkillPiece.SetParent(newSkillPieceParent);
                    var skillBoard = userData.GetEquipInstanceSkillBoard();
                    var skillPiece = uiElementSkillPiece.InstanceSkillPiece;
                    var placementSkillPiece = skillBoard.PlacementSkillPieces.First(x => x.InstanceSkillPieceId == skillPiece.InstanceId);
                    skillBoardBlackout.SetActive(false);
                    skillBoard.RemovePlacementSkillPiece(placementSkillPiece.InstanceSkillPieceId);
                    UpdateParameterLabels(userData.GetEquipInstanceSkillBoard().CreateInstanceCharacter(playerCharacterSpecId));
                    await BeginSkillPiecePlacementAsync(
                        placementSkillPiece.RotationIndex,
                        skillPiece.SkillPieceCellSpec.GetSize(placementSkillPiece.RotationIndex),
                        skillPiece,
                        uiElementSkillPiece,
                        cancellationToken
                        );
                    uiElementSkillPiece.Dispose();
                    uiElementSkillPiece = new UIElementSkillPiece(UnityEngine.Object.Instantiate(skillPiecePrefab, newSkillPieceParent));
                }
                else if (selectEditModeResult.winArgumentIndex == 2)
                {
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
            var instance = UnityEngine.Object.Instantiate(listContentPrefab, listParent);
            instance.Q<TMP_Text>("Text").SetText(text);
            cancellationToken.RegisterWithoutCaptureExecutionContext(() =>
            {
                if (instance != null)
                {
                    UnityEngine.Object.Destroy(instance.gameObject);
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
            foreach (var element in skillPieceElements)
            {
                element.Dispose();
            }
            skillPieceElements.Clear();
            skillBoardBackground.sizeDelta = new Vector2(
                instanceSkillBoard.SkillBoardSpec.X * 100,
                instanceSkillBoard.SkillBoardSpec.Y * 100
                );
            foreach (var hole in instanceSkillBoard.Holes)
            {
                var instance = new UIElementCell(UnityEngine.Object.Instantiate(cellPrefab, skillBoardBackground));
                instance.SetPosition(hole, instanceSkillBoard.SkillBoardSpec.X, instanceSkillBoard.SkillBoardSpec.Y);
                instance.SetBackgroundColor(Define.SkillPieceColor.Gray);
                holeElements.Add(instance);
            }
            foreach (var placementSkillPiece in instanceSkillBoard.PlacementSkillPieces)
            {
                var instanceSkillPiece = userData.GetInstanceSkillPiece(placementSkillPiece.InstanceSkillPieceId);
                var uiElementSkillPiece = new UIElementSkillPiece(UnityEngine.Object.Instantiate(skillPiecePrefab, skillBoardBackground));
                uiElementSkillPiece.Setup(instanceSkillPiece, placementSkillPiece.RotationIndex);
                uiElementSkillPiece.SetPosition(placementSkillPiece.PositionIndex, instanceSkillBoard.SkillBoardSpec.Size, instanceSkillPiece.SkillPieceCellSpec.GetSize(placementSkillPiece.RotationIndex));
                skillPieceElements.Add(uiElementSkillPiece);
            }
            var instanceCharacter = instanceSkillBoard.CreateInstanceCharacter(playerCharacterSpecId);
            UpdateParameterLabels(instanceCharacter);
        }

        private void UpdateParameterLabels(InstanceCharacter instanceCharacter)
        {
            hitPointLabel.SetText(instanceCharacter.CurrentHitPointMax.Current.ToString());
            physicalAttackLabel.SetText(instanceCharacter.CurrentPhysicalAttack.Current.ToString());
            physicalDefenseLabel.SetText(instanceCharacter.CurrentPhysicalDefense.Current.ToString());
            magicalAttackLabel.SetText(instanceCharacter.CurrentMagicalAttack.Current.ToString());
            magicalDefenseLabel.SetText(instanceCharacter.CurrentMagicalDefense.Current.ToString());
            speedLabel.SetText(instanceCharacter.CurrentSpeed.Current.ToString());
        }

        private static HKButton CreateHKButton(HKUIDocument document)
        {
            return new HKButton(document.Q<Button>("Button"));
        }

        private Vector2Int GetPositionIndexFromMousePosition(Vector2Int boardSize)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(skillBoardBackground, Mouse.current.position.ReadValue(), null, out var localPoint);
            var position = new Vector2(
                (localPoint.x + skillBoardBackground.sizeDelta.x * 0.5f) / 100.0f,
                (localPoint.y + skillBoardBackground.sizeDelta.y * 0.5f) / 100.0f
                );
            return new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        }

        private async UniTask<UIElementSkillPiece> GetClickedUIElementSkillPieceAsync(UIElementSkillPiece uiElementSkillPiece, CancellationToken cancellationToken)
        {
            var skillBoard = userData.GetEquipInstanceSkillBoard();
            UIElementSkillPiece placedUiElementSkillPiece = null;
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: cancellationToken);
                var positionIndex = GetPositionIndexFromMousePosition(skillBoard.SkillBoardSpec.Size);
                if (positionIndex.x < 0 || positionIndex.x >= skillBoard.SkillBoardSpec.X || positionIndex.y < 0 || positionIndex.y >= skillBoard.SkillBoardSpec.Y)
                {
                    placedUiElementSkillPiece?.SetParent(skillBoardBackground);
                    placedUiElementSkillPiece = null;
                    continue;
                }
                uiElementSkillPiece.Clear();
                UIElementSkillPiece cachedUiElementSkillPiece = null;
                foreach (var placementSkillPiece in skillBoard.PlacementSkillPieces)
                {
                    if (placementSkillPiece.ContainsPositionIndex(positionIndex, userData))
                    {
                        cachedUiElementSkillPiece = skillPieceElements.Find(x => x.InstanceSkillPiece.InstanceId == placementSkillPiece.InstanceSkillPieceId);
                        break;
                    }
                }
                if (placedUiElementSkillPiece != cachedUiElementSkillPiece)
                {
                    placedUiElementSkillPiece?.SetParent(skillBoardBackground);
                    placedUiElementSkillPiece = cachedUiElementSkillPiece;
                    placedUiElementSkillPiece?.SetParent(skillBoardArea.transform);
                }
                if (placedUiElementSkillPiece != null && playerInput.actions["UI/Submit"].WasPerformedThisFrame())
                {
                    return placedUiElementSkillPiece;
                }
            }

            return null;
        }

        private async UniTask BeginSkillPiecePlacementAsync(
            int rotationIndex,
            Vector2Int skillPieceSize,
            InstanceSkillPiece skillPiece,
            UIElementSkillPiece uiElementSkillPiece,
            CancellationToken cancellationToken
            )
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
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
                var positionIndex = uiElementSkillPiece.GetPositionIndexFromMousePosition(skillBoard.SkillBoardSpec.Size, skillPieceSize);
                uiElementSkillPiece.SetPosition(positionIndex, skillBoard.SkillBoardSpec.Size, skillPieceSize, new Vector2(-20.0f, 20.0f));
                if (playerInput.actions["UI/Submit"].WasPerformedThisFrame())
                {
                    if (!skillBoard.CanPlacementSkillPiece(userData, skillPiece, positionIndex, rotationIndex))
                    {
                        Debug.Log("スキルピース配置不可");
                    }
                    else
                    {
                        skillBoard.AddPlacementSkillPiece(skillPiece.InstanceId, positionIndex, rotationIndex, userData);
                        uiElementSkillPiece.SetPosition(positionIndex, skillBoard.SkillBoardSpec.Size, skillPieceSize, new Vector2(5.0f, -5.0f));
                        await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: cancellationToken);
                        uiElementSkillPiece.SetPosition(positionIndex, skillBoard.SkillBoardSpec.Size, skillPieceSize, new Vector2(0.0f, 0.0f));
                        await uiElementSkillPiece.PlayLineAnimationAsync(cancellationToken);
                        uiElementSkillPiece.Clear();
                        SetSkillBoard(skillBoard);
                        await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: cancellationToken);
                        break;
                    }
                }
                if (playerInput.actions["UI/Cancel"].WasPerformedThisFrame())
                {
                    Debug.Log("スキルピース配置キャンセル");
                    break;
                }
            }
        }
    }
}
