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
        [SerializeField]
        private ItemSpec.DictionaryList itemSpecs;
        public ItemSpec.DictionaryList ItemSpecs => itemSpecs;

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
                };
                var database = await UniTask.WhenAll(
                    masterDataNames.Select(GoogleSpreadSheetDownloader.DownloadAsync)
                );
                itemSpecs.Set(JsonHelper.FromJson<ItemSpec>(database[0]));
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

        [Serializable]
        public class WeaponSpec
        {
            public int Id;

            public string Name;

            [Serializable]
            public class DictionaryList : DictionaryList<int, WeaponSpec>
            {
                public DictionaryList() : base(x => x.Id)
                {
                }
            }
        }
    }
}