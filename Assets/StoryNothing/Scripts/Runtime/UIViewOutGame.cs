using System.Collections.Generic;
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
                var contents = new List<HKUIDocument>();
                var tasks = new List<UniTask>();
                var changeSkillBoardContent = CreateListContent("スキルボード変更");
                var selectBattleContent = CreateListContent("闘技場へ");
                contents.Add(changeSkillBoardContent);
                contents.Add(selectBattleContent);
                tasks.Add(GetButton(changeSkillBoardContent).OnClickAsync(cancellationToken));
                tasks.Add(GetButton(selectBattleContent).OnClickAsync(cancellationToken));
                var result = await UniTask.WhenAny(tasks);
                foreach (var content in contents)
                {
                    Object.Destroy(content.gameObject);
                }
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

        private UniTask StateChangeSkillBoardAsync(CancellationToken cancellationToken)
        {
            return UniTask.Never(cancellationToken);
        }

        private UniTask StateSelectBattleAsync(CancellationToken cancellationToken)
        {
            return UniTask.Never(cancellationToken);
        }

        private HKUIDocument CreateListContent(string text)
        {
            var instance = Object.Instantiate(listContentPrefab, listParent);
            instance.Q<TMP_Text>("Text").SetText(text);
            return instance;
        }

        private static Button GetButton(HKUIDocument document)
        {
            return document.Q<Button>("Button");
        }
    }
}
