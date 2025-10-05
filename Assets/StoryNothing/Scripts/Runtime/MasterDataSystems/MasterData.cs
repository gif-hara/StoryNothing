using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using R3;
using UnityEditor;
using UnityEngine;

namespace StoryNothing.MasterDataSystems
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "MasterData", menuName = "StoryNothing/MasterData")]
    public sealed class MasterData : ScriptableObject
    {
        [field: SerializeField]
        public ItemSpec.DictionaryList ItemSpecs { get; private set; }

        [field: SerializeField]
        public CharacterSpec.DictionaryList CharacterSpecs { get; private set; }

        [field: SerializeField]
        public SkillBoardSpec.DictionaryList SkillBoardSpecs { get; private set; }

        [field: SerializeField]
        public SkillPieceSpec.DictionaryList SkillPieceSpecs { get; private set; }

        [field: SerializeField]
        public SkillPieceCellSpec.DictionaryList SkillPieceCellSpecs { get; private set; }

        [field: SerializeField]
        public SkillPieceCellPoint.Group SkillPieceCellPoints { get; private set; }

        [field: SerializeField]
        public SkillSpec.DictionaryList SkillSpecs { get; private set; }

        [field: SerializeField]
        public SkillAttachGroup.Group SkillAttachGroups { get; private set; }

        [field: SerializeField]
        public CreateSkillPieceSpec.DictionaryList CreateSkillPieceSpecs { get; private set; }

        [field: SerializeField]
        public SkillEffect.Group SkillEffects { get; private set; }

#if UNITY_EDITOR
        [ContextMenu("Update")]
        private async void UpdateMasterData()
        {
            var startTime = DateTime.Now;
            var progressId = UnityEditor.Progress.Start("MasterData Update");
            var scope = new CancellationTokenSource();
            try
            {
                Observable.EveryUpdate()
                    .Subscribe(_ =>
                    {
                        var elapsed = DateTime.Now - startTime;
                        UnityEditor.Progress.Report(progressId, (float)elapsed.TotalSeconds / 60.0f);
                    })
                    .RegisterTo(scope.Token);
                Debug.Log("Begin MasterData Update");
                var masterDataNames = new[]
                {
                    "ItemSpec",
                    "SkillBoardSpec",
                    "SkillPieceSpec",
                    "SkillPieceCellSpec",
                    "SkillPieceCellPoint",
                    "CharacterSpec",
                    "SkillSpec",
                    "SkillAttachGroup",
                    "CreateSkillPieceSpec",
                    "SkillEffect",
                };
                var database = await UniTask.WhenAll(
                    masterDataNames.Select(GoogleSpreadSheetDownloader.DownloadAsync)
                );
                ItemSpecs.Set(JsonHelper.FromJson<ItemSpec>(database[0]));
                SkillBoardSpecs.Set(JsonHelper.FromJson<SkillBoardSpec>(database[1]));
                SkillPieceSpecs.Set(JsonHelper.FromJson<SkillPieceSpec>(database[2]));
                SkillPieceCellSpecs.Set(JsonHelper.FromJson<SkillPieceCellSpec>(database[3]));
                SkillPieceCellPoints.Set(JsonHelper.FromJson<SkillPieceCellPoint>(database[4]));
                CharacterSpecs.Set(JsonHelper.FromJson<CharacterSpec>(database[5]));
                SkillSpecs.Set(JsonHelper.FromJson<SkillSpec>(database[6]));
                SkillAttachGroups.Set(JsonHelper.FromJson<SkillAttachGroup>(database[7]));
                CreateSkillPieceSpecs.Set(JsonHelper.FromJson<CreateSkillPieceSpec>(database[8]));
                SkillEffects.Set(JsonHelper.FromJson<SkillEffect>(database[9]));
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                Debug.Log("End MasterData Update");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                UnityEditor.Progress.Remove(progressId);
                scope.Cancel();
                scope.Dispose();
            }
        }
#endif
    }
}