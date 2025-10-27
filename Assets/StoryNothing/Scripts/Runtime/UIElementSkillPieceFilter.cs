using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using HK;
using R3;
using StoryNothing.MasterDataSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StoryNothing
{
    public sealed class UIElementSkillPieceFilter
    {
        private readonly HKUIDocument document;

        private readonly TMP_Text cellNumberValue;

        private readonly TMP_Text colorValue;

        private readonly TMP_Text cellNameValue;

        private readonly Dictionary<int, Button> buttonCellNumbers = new();

        private readonly Dictionary<Define.SkillPieceColor, Button> buttonColors = new();

        private readonly Dictionary<int, Dictionary<string, Button>> buttonCellNames = new();

        private readonly GameObject buttonCellNameArea;

        private readonly Dictionary<int, GameObject> buttonCellNameRoots = new();

        private readonly Button buttonSubmit;

        public UIElementSkillPieceFilter(HKUIDocument document)
        {
            this.document = document;
            document.gameObject.SetActive(false);
            var valueArea = document.Q<HKUIDocument>("Area.Value");
            cellNumberValue = valueArea.Q<TMP_Text>("CellNumber");
            colorValue = valueArea.Q<TMP_Text>("Color");
            cellNameValue = valueArea.Q<TMP_Text>("PieceType");
            var buttonArea = document.Q<HKUIDocument>("Area.Button");
            var buttonCellNumberArea = buttonArea.Q<HKUIDocument>("CellNumber");
            var skillPieceCellSpecs = ServiceLocator.Resolve<MasterData>().SkillPieceCellSpecs;
            foreach (var cellNumber in skillPieceCellSpecs.List.GroupBy(spec => spec.GroupId).Select(x => x.Key))
            {
                var button = buttonCellNumberArea.Q<HKUIDocument>($"UI.Element.Button.CellNumber.{cellNumber}").Q<Button>("Button");
                buttonCellNumbers[cellNumber] = button;
            }
            buttonCellNumbers[0] = buttonCellNumberArea.Q<HKUIDocument>("UI.Element.Button.CellNumber.None").Q<Button>("Button");
            var buttonColorArea = buttonArea.Q<HKUIDocument>("Color");
            foreach (var i in Enum.GetValues(typeof(Define.SkillPieceColor)))
            {
                var color = (Define.SkillPieceColor)i;
                var colorName = color == Define.SkillPieceColor.Gray ? "None" : color.ToString();
                var button = buttonColorArea.Q<HKUIDocument>($"UI.Element.Button.Color.{colorName}").Q<Button>("Button");
                buttonColors[color] = button;
            }
            var buttonCellNameArea = buttonArea.Q<HKUIDocument>("PieceType");
            this.buttonCellNameArea = buttonCellNameArea.gameObject;
            foreach (var groupedSkillPieceCellSpecs in skillPieceCellSpecs.List.GroupBy(spec => spec.GroupId))
            {
                var cellNumber = groupedSkillPieceCellSpecs.Key;
                var dict = new Dictionary<string, Button>();
                var cellNumberArea = buttonCellNameArea.Q<HKUIDocument>(cellNumber.ToString());
                if (cellNumberArea == null)
                {
                    continue;
                }
                buttonCellNameRoots[cellNumber] = cellNumberArea.gameObject;

                foreach (var spec in groupedSkillPieceCellSpecs)
                {
                    var button = cellNumberArea
                        .Q<HKUIDocument>($"UI.Element.Button.PieceType.{spec.Name}")
                        .Q<Button>("Button");
                    dict[spec.Name] = button;
                }

                var noneButtonDocument = cellNumberArea.Q<HKUIDocument>("UI.Element.Button.PieceType.None");
                if (noneButtonDocument != null)
                {
                    dict["None"] = noneButtonDocument.Q<Button>("Button");
                }
                buttonCellNames[cellNumber] = dict;
            }
            buttonSubmit = document.Q<HKUIDocument>("UI.Element.Button.Submit").Q<Button>("Button");
        }

        public UniTask<SkillPieceFilterData> ShowAsync(SkillPieceFilterData oldFilterData, CancellationToken cancellationToken)
        {
            document.gameObject.SetActive(true);
            var result = oldFilterData == null ? new SkillPieceFilterData() : new SkillPieceFilterData(oldFilterData);
            cellNumberValue.text = result.cellNumber == 0 ? "なし" : result.cellNumber.ToString();
            colorValue.text = result.color == Define.SkillPieceColor.Gray ? "なし" : result.color.LocalizedName();
            cellNameValue.text = string.IsNullOrEmpty(result.cellName) ? "なし" : result.cellName;
            buttonCellNameArea.SetActive(result.cellNumber != 0);
            if (result.cellNumber == 0)
            {
                cellNameValue.text = "なし";
            }
            foreach (var root in buttonCellNameRoots)
            {
                root.Value.SetActive(root.Key == result.cellNumber);
            }

            var source = new UniTaskCompletionSource<SkillPieceFilterData>();
            buttonCellNumbers.Select(x => x.Value.OnClickAsObservable().Select(_ => x.Key))
                .Merge()
                .Subscribe((this, result), static (cellNumber, t) =>
                {
                    var (@this, result) = t;
                    if (result.cellNumber == cellNumber)
                    {
                        return;
                    }
                    result.cellNumber = cellNumber;
                    result.cellName = string.Empty;
                    @this.cellNameValue.text = "なし";
                    @this.cellNumberValue.text = cellNumber == 0 ? "なし" : cellNumber.ToString();
                    @this.buttonCellNameArea.SetActive(cellNumber != 0);
                    if (cellNumber == 0)
                    {
                        result.cellName = string.Empty;
                        @this.cellNameValue.text = "なし";
                    }
                    foreach (var root in @this.buttonCellNameRoots)
                    {
                        root.Value.SetActive(root.Key == cellNumber);
                    }
                })
                .RegisterTo(cancellationToken);
            buttonColors.Select(x => x.Value.OnClickAsObservable().Select(_ => x.Key))
                .Merge()
                .Subscribe((this, result), static (color, t) =>
                {
                    var (@this, result) = t;
                    if (result.color == color)
                    {
                        return;
                    }
                    result.color = color;
                    @this.colorValue.text = color == Define.SkillPieceColor.Gray ? "なし" : color.LocalizedName();
                })
                .RegisterTo(cancellationToken);
            buttonCellNames.SelectMany(group => group.Value.Select(x => x.Value.OnClickAsObservable().Select(_ => x.Key)))
                .Merge()
                .Subscribe((this, result), static (pieceName, t) =>
                {
                    var (@this, result) = t;
                    if (result.cellName == pieceName)
                    {
                        return;
                    }
                    result.cellName = pieceName == "None" ? string.Empty : pieceName;
                    @this.cellNameValue.text = pieceName == "None" ? "なし" : pieceName;
                })
                .RegisterTo(cancellationToken);
            buttonSubmit.OnClickAsObservable()
                .Subscribe((this, result, source), static (_, t) =>
                {
                    var (@this, result, source) = t;
                    @this.document.gameObject.SetActive(false);
                    source.TrySetResult(result);
                })
                .RegisterTo(cancellationToken);

            return source.Task;
        }
    }
}
