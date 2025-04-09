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
        private readonly List<PaintArea> areasCollided = new(); //接触リスト

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
                areasNotActive[0].Activate();
                areasActive.Add(areasNotActive[0]);
                areasNotActive.RemoveAt(0);
            }
        }

        /// <summary>
        /// 各エリアと塗り領域が接触しているか確認し，それらを塗って次のエリアを有効にする．
        /// </summary>
        public void UpdateAreas(Vector2 paintPosStart, Vector2 paintPosEnd)
        {
            //有効エリアと無効エリアの総数．
            int areaNumberRemain = areasActive.Count + areasNotActive.Count;
            //塗り領域と接触しているすべてのエリアを接触リストに移動する．
            for (int i = areasActive.Count - 1; i >= 0; i--)
                if (areasActive[i].IsColliding(paintPosStart, paintPosEnd))
                    ShiftAreaToCollidedList(areasActive[i]);
            PaintAreas(paintPosStart, paintPosEnd);
            areasCollided.Clear();
            ActivateNewAreas();
            //塗り領域がどのエリアとも接触していない場合．
            if (areaNumberRemain == areasActive.Count + areasNotActive.Count)
                Debug.Log("BAD");
        }

        /// <summary>
        /// 塗り領域と接触したエリアを有効リストから接触リストに移動させる．
        /// </summary>
        private void ShiftAreaToCollidedList(PaintArea area)
        {
            areasCollided.Add(area);
            areasActive.Remove(area);
        }

        /// <summary>
        /// 接触リスト内のエリアをすべて塗る．
        /// </summary>
        private void PaintAreas(Vector2 paintPosStart, Vector2 paintPosEnd)
        {
            if (areasCollided.Count == 0)
                return;
            //接触リスト内の各エリアを塗り領域の開始座標からの距離でソートする．
            areasCollided.Sort((a, b) => 
            a.GetDistFromStart(paintPosStart).CompareTo(b.GetDistFromStart(paintPosStart)));
            //連鎖したエリアのうち最後以外のものを，塗り領域の開始座標を更新しながら塗る．
            Vector2 paintPosStartCurrent = paintPosStart;
            for (int i = 0; i < areasCollided.Count - 1; i++)
            {
                areasCollided[i].PaintInChain(paintPosStartCurrent, paintPosEnd);
                paintPosStartCurrent = areasCollided[i].GetEdgePos(paintPosStartCurrent, paintPosEnd);
            }
            //最後のエリアを塗る．
            areasCollided[^1].PaintOnEndOfChain(paintPosStartCurrent, paintPosEnd);
        }
    }
}
