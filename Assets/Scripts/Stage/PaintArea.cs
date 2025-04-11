using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Stage
{
    public class PaintArea : MonoBehaviour
    {
        [SerializeField] private List<InkUnit> inkUnits = new(); //領域内のインクユニットのリスト
        public List<InkUnit> InkUnits => inkUnits;

        /// <summary>
        /// エリアのインクユニットを透明にする．
        /// </summary>
        public void Activate()
        {
            for(int i = 0; i < inkUnits.Count; i++)
                inkUnits[i].GetComponent<SpriteRenderer>().color = Color.clear; //変更予定．
        }

        /// <summary>
        /// エリア内に塗られたユニットが存在するか判定する．
        /// </summary>
        public bool IsPainted()
        {
            for(int i = 0; i < inkUnits.Count; i++)
                if (InkUnitStorage.IsPainted(inkUnits[i]))
                    return true;
            return false;
        }

        /// <summary>
        /// 塗り領域との接触領域によって評価する．
        /// </summary>
        public void Evaluate()
        {
            int inkUnitPaintedCount = 0;
            for(int i = 0; i < inkUnits.Count; i++)
                if (InkUnitStorage.IsPainted(inkUnits[i]))
                    inkUnitPaintedCount++;
            if (inkUnitPaintedCount == inkUnits.Count)
                Debug.Log("PERFECT");
            else
                Debug.Log("GOOD");
        }
    }
}