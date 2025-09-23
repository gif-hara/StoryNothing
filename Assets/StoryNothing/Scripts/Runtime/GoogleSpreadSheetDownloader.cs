using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace HK
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GoogleSpreadSheetDownloader
    {
        const string url = "https://script.google.com/macros/s/AKfycbwyE1I1aYwZTR_5vBR5SsWChfNffMpiaKmMeFlHtqL3anQKGUtc3YQN-vBdx6TN-AlYGg/exec";

        public static async UniTask<string> DownloadAsync(string sheetName)
        {
            var request = UnityWebRequest.Get(url + "?sheetName=" + sheetName);
            request.timeout = 60;
            try
            {
                await request.SendWebRequest();
                return request.downloadHandler.text;
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"sheetName: {sheetName}{System.Environment.NewLine}{e.Message}");
                return null;
            }
        }
    }
}