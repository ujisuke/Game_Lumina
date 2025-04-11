using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Stage
{
    public static class InkUnitStorage
    {
        private static readonly List<InkUnit> inkUnits = new(); //インクユニットのリスト
        private static readonly List<InkUnit> inkUnitsPainted = new(); //塗られたインクユニットのリスト

        /// <summary>
        /// リストを初期化する．
        /// </summary>
        public static void Initialize()
        {
            for(int i = 0; i < inkUnitsPainted.Count; i++)
                inkUnits.Add(inkUnitsPainted[i]);
            inkUnitsPainted.Clear();
        }

        /// <summary>
        /// インクユニットをリストに登録する．
        /// </summary>
        public static void AddInkUnit(InkUnit inkUnit)
        {
            inkUnits.Add(inkUnit);
        }

        /// <summary>
        /// 塗られるべきインクユニットを探し，塗り，別のリストに移動させる．
        /// </summary>
        public static void PaintInkUnit(Vector2 playerPaintPos)
        {
            for (int i = 0; i < inkUnits.Count; i++)
            {
                if (!inkUnits[i].IsPainted(playerPaintPos))
                    continue;
                inkUnitsPainted.Add(inkUnits[i]);
                inkUnits[i].Paint();
                inkUnits.RemoveAt(i);
            }
        }

        /// <summary>
        /// インクユニットが塗られたか判定する．
        /// </summary>
        public static bool IsPainted(InkUnit inkUnit)
        {
            return inkUnitsPainted.Contains(inkUnit);
        }

        /// <summary>
        /// 有効エリア外で塗られたインクユニットが存在するか判定する．
        /// </summary>
        public static bool IsPaintedOutsideOfAreas(List<PaintArea> areasPainted)
        {
            List<InkUnit> inkUnitsPaintedOutsideOfAreas = GetInkUnitsPaintedOutsideOfAreas(areasPainted);
            return inkUnitsPaintedOutsideOfAreas.Count > 0;
        }

        /// <summary>
        /// 有効エリア外で塗られたインクユニットのリストを返す．
        /// </summary>
        private static List<InkUnit> GetInkUnitsPaintedOutsideOfAreas(List<PaintArea> areasPainted)
        {
            List<InkUnit> inkUnitsInsideAreas = new();
            for (int i = 0; i < areasPainted.Count; i++)
            {
                List<InkUnit> inkUnitsInsideArea = areasPainted[i].InkUnits;
                for (int j = 0; j < inkUnitsInsideArea.Count; j++)
                    inkUnitsInsideAreas.Add(inkUnitsInsideArea[j]);
            }
            List<InkUnit> inkUnitsPaintedOutsideOfAreas = new();
            for (int i = 0; i < inkUnitsPainted.Count; i++)
                if (!inkUnitsInsideAreas.Contains(inkUnitsPainted[i]))
                    inkUnitsPaintedOutsideOfAreas.Add(inkUnitsPainted[i]);
            return inkUnitsPaintedOutsideOfAreas;
        }

        /// <summary>
        /// 有効エリア外で塗られるべきインクユニットを塗る．
        /// </summary>
        public static void PaintInkUnitError(List<PaintArea> areasPainted)
        {
            List<InkUnit> inkUnitsPaintedOutsideOfAreas = GetInkUnitsPaintedOutsideOfAreas(areasPainted);
            for (int i = 0; i < inkUnitsPaintedOutsideOfAreas.Count; i++)
                inkUnitsPaintedOutsideOfAreas[i].PaintError();
        }
    }
}
