using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using R3;
using StoryNothing.InstanceData;
using StoryNothing.MasterDataSystems;
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

        private readonly TMP_Text attackLabel;

        private readonly TMP_Text defenseLabel;

        private readonly TMP_Text speedLabel;

        private readonly TMP_Text instanceSkillPieceNameLabel;

        private readonly HKUIDocument instanceSkillPieceSkillLabelPrefab;

        private readonly List<HKUIDocument> instanceSkillPieceSkillLabels = new();

        private readonly Transform instanceSkillPieceSkillLabelParent;

        private readonly GameObject instanceSkillPieceInformationArea;

        private readonly HKUIDocument skillNameLabelPrefab;

        private readonly List<HKUIDocument> skillNameLabels = new();

        private readonly Transform skillNameLabelParent;

        private readonly HKUIDocument bingoBonusSkillLabelPrefab;

        private readonly List<HKUIDocument> bingoBonusSkillLabels = new();

        private readonly Transform bingoBonusSkillLabelParent;

        private readonly TMP_Text bingoBonusHeaderLabel;

        private readonly HKUIDocument bingoBonusLinePrefab;

        private readonly RectTransform bingoBonusLineParent;

        private readonly List<UIElementBingoLine> bingoBonusLines = new();

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
            attackLabel = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<HKUIDocument>("UI.Element.Parameter.Attack")
                .Q<TMP_Text>("Value");
            defenseLabel = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<HKUIDocument>("UI.Element.Parameter.Defense")
                .Q<TMP_Text>("Value");
            speedLabel = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<HKUIDocument>("UI.Element.Parameter.Speed")
                .Q<TMP_Text>("Value");
            instanceSkillPieceNameLabel = this.document
                .Q<HKUIDocument>("Area.Center")
                .Q<HKUIDocument>("Area.Information")
                .Q<HKUIDocument>("Area.SkillPiece")
                .Q<TMP_Text>("Name");
            instanceSkillPieceSkillLabelPrefab = this.document
                .Q<HKUIDocument>("UI.Element.Message");
            skillNameLabelPrefab = this.document
                .Q<HKUIDocument>("UI.Element.Message");
            instanceSkillPieceSkillLabelParent = this.document
                .Q<HKUIDocument>("Area.Center")
                .Q<HKUIDocument>("Area.Information")
                .Q<HKUIDocument>("Area.SkillPiece")
                .Q<Transform>("Skills");
            instanceSkillPieceInformationArea = this.document
                .Q<HKUIDocument>("Area.Center")
                .Q<HKUIDocument>("Area.Information")
                .Q("Area.SkillPiece");
            skillNameLabelParent = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<Transform>("Skills");
            bingoBonusSkillLabelPrefab = this.document
                .Q<HKUIDocument>("UI.Element.Message");
            bingoBonusSkillLabelParent = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<Transform>("BingoBonuses");
            bingoBonusHeaderLabel = this.document
                .Q<HKUIDocument>("Area.Right")
                .Q<TMP_Text>("Header.BingoBonus");
            bingoBonusLinePrefab = this.document
                .Q<HKUIDocument>("UI.Element.BingoLine");
            bingoBonusLineParent = this.document
                .Q<HKUIDocument>("Area.Center")
                .Q<HKUIDocument>("Area.SkillBoard")
                .Q<RectTransform>("BingoBonus");
        }

        public async UniTask BeginAsync(CancellationToken cancellationToken)
        {
            await StateRootAsync(cancellationToken);
        }

        private async UniTask StateRootAsync(CancellationToken cancellationToken)
        {
            document.gameObject.SetActive(true);
            SetActiveInstanceSkillPieceInformation(false);
            SetupSkillBoard(userData.GetEquipInstanceSkillBoard());
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
                            .OnPointerEnter(hkButton => SetupSkillBoard(x.Value))
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
                SetupSkillBoard(userData.GetEquipInstanceSkillBoard());
            }
            scope.Cancel();
            scope.Dispose();
        }

        private async UniTask StateEditSkillBoardAsync(CancellationToken cancellationToken)
        {
            var uiElementSkillPiece = new UIElementSkillPiece(UnityEngine.Object.Instantiate(skillPiecePrefab, newSkillPieceParent));
            SetActiveInstanceSkillPieceInformation(true);
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
                                    SetupSkillPieceInformation(x);
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
                    SetupBingoBonusLine(skillBoard);
                    UpdateParameterLabels(skillBoard);
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
            SetActiveInstanceSkillPieceInformation(false);
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

        private void SetupSkillBoard(InstanceSkillBoard instanceSkillBoard)
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
            foreach (var placementSkillPiece in instanceSkillBoard.PlacementSkillPieces)
            {
                var instanceSkillPiece = userData.GetInstanceSkillPiece(placementSkillPiece.InstanceSkillPieceId);
                var uiElementSkillPiece = new UIElementSkillPiece(UnityEngine.Object.Instantiate(skillPiecePrefab, skillBoardBackground));
                uiElementSkillPiece.Setup(instanceSkillPiece, placementSkillPiece.RotationIndex);
                uiElementSkillPiece.SetPosition(placementSkillPiece.PositionIndex, instanceSkillBoard.SkillBoardSpec.Size, instanceSkillPiece.SkillPieceCellSpec.GetSize(placementSkillPiece.RotationIndex));
                skillPieceElements.Add(uiElementSkillPiece);
            }
            skillBoardBackground.sizeDelta = new Vector2(
                instanceSkillBoard.SkillBoardSpec.X * Define.CellSize,
                instanceSkillBoard.SkillBoardSpec.Y * Define.CellSize
                );
            foreach (var hole in instanceSkillBoard.Holes)
            {
                var instance = new UIElementCell(UnityEngine.Object.Instantiate(cellPrefab, skillBoardBackground));
                instance.SetPosition(hole, instanceSkillBoard.SkillBoardSpec.X, instanceSkillBoard.SkillBoardSpec.Y);
                instance.SetBackgroundColor(Define.SkillPieceColor.Gray);
                holeElements.Add(instance);
            }
            SetupBingoBonusLine(instanceSkillBoard);
            UpdateParameterLabels(instanceSkillBoard);
        }

        private void SetupBingoBonusLine(InstanceSkillBoard instanceSkillBoard)
        {
            foreach (var line in bingoBonusLines)
            {
                line.Dispose();
            }
            bingoBonusLines.Clear();
            bingoBonusLineParent.sizeDelta = new Vector2(
                instanceSkillBoard.SkillBoardSpec.X * Define.CellSize,
                instanceSkillBoard.SkillBoardSpec.Y * Define.CellSize
                );
            foreach (var bingoIndex in instanceSkillBoard.GetHorizontalBingoIndexes(userData))
            {
                var line = new UIElementBingoLine(UnityEngine.Object.Instantiate(bingoBonusLinePrefab, bingoBonusLineParent));
                line.SetupAsHorizontalLine(bingoIndex, instanceSkillBoard.SkillBoardSpec.Y);
                bingoBonusLines.Add(line);
            }
            foreach (var bingoIndex in instanceSkillBoard.GetVerticalBingoIndexes(userData))
            {
                var line = new UIElementBingoLine(UnityEngine.Object.Instantiate(bingoBonusLinePrefab, bingoBonusLineParent));
                line.SetupAsVerticalLine(bingoIndex, instanceSkillBoard.SkillBoardSpec.X);
                bingoBonusLines.Add(line);
            }
        }

        private void UpdateParameterLabels(InstanceSkillBoard instanceSkillBoard)
        {
            var instanceCharacter = instanceSkillBoard.CreateInstanceCharacter(playerCharacterSpecId, userData);
            hitPointLabel.SetText(instanceCharacter.CurrentHitPointMax.Current.ToString());
            attackLabel.SetText(instanceCharacter.CurrentAttack.Current.ToString());
            defenseLabel.SetText(instanceCharacter.CurrentDefense.Current.ToString());
            speedLabel.SetText(instanceCharacter.CurrentSpeed.Current.ToString());

            foreach (var label in skillNameLabels)
            {
                UnityEngine.Object.Destroy(label.gameObject);
            }
            skillNameLabels.Clear();
            foreach (var label in bingoBonusSkillLabels)
            {
                UnityEngine.Object.Destroy(label.gameObject);
            }
            bingoBonusSkillLabels.Clear();

            var skillSpecs = instanceSkillBoard.PlacementSkillPieces
                .SelectMany(x => userData.GetInstanceSkillPiece(x.InstanceSkillPieceId).SkillSpecIds)
                .Select(x => ServiceLocator.Resolve<MasterData>().SkillSpecs.Get(x))
                .GroupBy(x => x.Id);
            foreach (var skillGroup in skillSpecs)
            {
                var skillNameLabel = UnityEngine.Object.Instantiate(skillNameLabelPrefab, skillNameLabelParent);
                skillNameLabel.Q<TMP_Text>("Text").SetText($"[{skillGroup.Count()}] {skillGroup.First().Name}");
                skillNameLabels.Add(skillNameLabel);
            }

            var bingoBonusCount = instanceSkillBoard.GetHorizontalBingoIndexes(userData).Count +
                instanceSkillBoard.GetVerticalBingoIndexes(userData).Count;
            bingoBonusHeaderLabel.SetText($"ビンゴボーナス: [{bingoBonusCount}]");
            for (var i = 0; i < instanceSkillBoard.BingoBonusSkillSpecIds.Count; i++)
            {
                var skillSpecId = instanceSkillBoard.BingoBonusSkillSpecIds[i];
                var skillSpec = ServiceLocator.Resolve<MasterData>().SkillSpecs.Get(skillSpecId);
                var bingoBonusSkillLabel = UnityEngine.Object.Instantiate(bingoBonusSkillLabelPrefab, bingoBonusSkillLabelParent);
                var text = bingoBonusSkillLabel.Q<TMP_Text>("Text");
                text.SetText(skillSpec.Name);
                text.color = i < bingoBonusCount ? Color.yellow : Color.black;
                bingoBonusSkillLabels.Add(bingoBonusSkillLabel);
            }
        }

        private static HKButton CreateHKButton(HKUIDocument document)
        {
            return new HKButton(document.Q<Button>("Button"));
        }

        private Vector2Int GetPositionIndexFromMousePosition(Vector2Int boardSize)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(skillBoardBackground, Mouse.current.position.ReadValue(), null, out var localPoint);
            var position = new Vector2(
                (localPoint.x + skillBoardBackground.sizeDelta.x * 0.5f) / Define.CellSize,
                (localPoint.y + skillBoardBackground.sizeDelta.y * 0.5f) / Define.CellSize
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
                    SetupSkillPieceInformation(placedUiElementSkillPiece?.InstanceSkillPiece);
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
                        SetupSkillBoard(skillBoard);
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

        private void SetupSkillPieceInformation(InstanceSkillPiece instanceSkillPiece)
        {
            if (instanceSkillPiece == null)
            {
                SetActiveInstanceSkillPieceInformation(false);
                return;
            }
            SetActiveInstanceSkillPieceInformation(true);
            instanceSkillPieceNameLabel.SetText(instanceSkillPiece.Name);
            foreach (var label in instanceSkillPieceSkillLabels)
            {
                UnityEngine.Object.Destroy(label.gameObject);
            }
            instanceSkillPieceSkillLabels.Clear();
            foreach (var skillSpecId in instanceSkillPiece.SkillSpecIds)
            {
                var skillSpec = ServiceLocator.Resolve<MasterData>().SkillSpecs.Get(skillSpecId);
                var messageInstance = UnityEngine.Object.Instantiate(instanceSkillPieceSkillLabelPrefab, instanceSkillPieceSkillLabelParent);
                messageInstance.Q<TMP_Text>("Text").SetText(skillSpec.Name);
                instanceSkillPieceSkillLabels.Add(messageInstance);
            }
        }

        private void SetActiveInstanceSkillPieceInformation(bool isActive)
        {
            instanceSkillPieceInformationArea.SetActive(isActive);
        }
    }
}
