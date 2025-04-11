using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Stage
{
    public class PaintAreaStorage : MonoBehaviour
    {
        [SerializeField] private List<PaintArea> areas; //すべてのエリアのリスト
        [SerializeField] private int areaNumberActive = 0; //有効エリアの最大数
        private readonly List<PaintArea> areasActive = new(); //有効リスト
        private readonly List<PaintArea> areasNotActive = new(); //無効リスト
        private readonly List<PaintArea> areasPainted = new(); //塗りリスト

        /// <summary>
        /// 有効リストと無効リストに各エリアを追加する．
        /// </summary>
        private void Awake()
        {
            //最初にすべてのエリアを無効リストに追加し，その一部を有効リストに移動させる．
            for (int i = 0; i < areas.Count; i++)
                areasNotActive.Add(areas[i]);
            ActivateNewAreas();
        }

        /// <summary>
        /// 無効エリアを無効リストから順番に有効リストに移動させ，有効化する．
        /// </summary>
        private void ActivateNewAreas()
        {
            int areaNumberNextActivate = areaNumberActive - areasActive.Count;
            for(int i = 0; i < areaNumberNextActivate; i++)
            {
                if (areasNotActive.Count == 0)
                    return;
                areasActive.Add(areasNotActive[0]);
                areasNotActive.RemoveAt(0);
                areasActive[^1].Activate(); 
            }
        }

        /// <summary>
        /// 各塗りエリアを評価し，次のエリアを有効にする．
        /// </summary>
        public void UpdateAreas()
        {
            for (int i = areasActive.Count - 1; i >= 0; i--)
                if (areasActive[i].IsPainted())
                    ShiftToPaintedList(areasActive[i]);
            EvaluatePaintedAreas();
            areasPainted.Clear();
            ActivateNewAreas();
            InkUnitStorage.Initialize(); 
        }

        /// <summary>
        /// エリアを有効リストから塗りリストに移動させる．
        /// </summary>
        private void ShiftToPaintedList(PaintArea areaActive)
        {
            areasPainted.Add(areaActive);
            areasActive.Remove(areaActive);
        }

        /// <summary>
        /// 塗りリスト内のエリアをすべて評価する．
        /// </summary>
        private void EvaluatePaintedAreas()
        {
            if(InkUnitStorage.IsPaintedOutsideOfAreas(areasPainted))
                EvaluateError();
            else
                for (int i = 0; i < areasPainted.Count; i++)
                    areasPainted[i].Evaluate();
        }

        /// <summary>
        /// 塗りリスト内のエリアの外にある塗ユニットをすべて評価する．
        /// </summary>
        private void EvaluateError()
        {
            Debug.Log("BAD");
            InkUnitStorage.PaintInkUnitError(areasPainted);
        }
    }
}
