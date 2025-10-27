using System;

namespace StoryNothing
{
    public static partial class Extensions
    {
        public static string LocalizedName(this Define.SkillPieceColor color)
        {
            return color switch
            {
                Define.SkillPieceColor.Red => "赤",
                Define.SkillPieceColor.Orange => "橙",
                Define.SkillPieceColor.WhiteGray => "白灰",
                Define.SkillPieceColor.Green => "緑",
                Define.SkillPieceColor.Gray => "なし",
                _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
            };
        }
    }
}
