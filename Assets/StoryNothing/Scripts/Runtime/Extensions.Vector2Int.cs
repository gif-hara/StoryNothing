using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK
{
    public static partial class Extensions
    {
        public static List<Vector2Int> RotateClockwise(this List<Vector2Int> points, int rotationIndex)
        {
            Assert.IsTrue(rotationIndex >= 0);
            Assert.IsTrue(rotationIndex <= 3);
            for (var i = 0; i < points.Count; i++)
            {
                var rotatedPoint = points[i];
                for (var j = 0; j < rotationIndex; j++)
                {
                    rotatedPoint = new Vector2Int(-rotatedPoint.y, rotatedPoint.x);
                }
                points[i] = rotatedPoint;
            }
            return points;
        }
    }
}