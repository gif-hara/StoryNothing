using System;
using System.Collections.Generic;
using System.Linq;
using HK;
using UnityEngine;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillPieceCellSpec
    {
        public int Id;

        public int GroupId;

        public List<Vector2Int> GetCellPoints(int rotationIndex)
        {
            var points = ServiceLocator.Resolve<MasterData>().SkillPieceCellPoints.Get(Id).Select(x => new Vector2Int(x.X, x.Y)).ToList();
            points = points.RotateClockwise(rotationIndex);

            // 負数がある場合、(0,0)基準にする
            var xMin = points.Count > 0 ? points.Min(x => x.x) : 0;
            var yMin = points.Count > 0 ? points.Min(x => x.y) : 0;
            if (xMin < 0 || yMin < 0)
            {
                for (var i = 0; i < points.Count; i++)
                {
                    points[i] = new Vector2Int(points[i].x - xMin, points[i].y - yMin);
                }
            }
            return points;
        }

        public Vector2Int GetSize(int rotationIndex)
        {
            var cellPoints = GetCellPoints(rotationIndex);
            var xMax = cellPoints.Count > 0 ? cellPoints.Max(x => x.x) + 1 : 0;
            var yMax = cellPoints.Count > 0 ? cellPoints.Max(x => x.y) + 1 : 0;
            return new Vector2Int(xMax, yMax);
        }

        [Serializable]
        public sealed class DictionaryList : DictionaryList<int, SkillPieceCellSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
