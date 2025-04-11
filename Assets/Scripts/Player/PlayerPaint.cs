using Assets.Scripts.Stage;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerPaint : MonoBehaviour
    {
        [SerializeField] private PaintAreaStorage paintAreaStorage; //すべてのエリアを管理
        private bool isPainting = false; //塗り中かどうか
        
        /// <summary>
        /// クリックしていたら塗る．
        /// </summary>
        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0) && !isPainting)
                StartPaint();
            if (Input.GetMouseButton(0) && isPainting)
                ContinuePaint();
            if (!Input.GetMouseButton(0) && isPainting)
                EndPaint();
        }

        /// <summary>
        /// 塗り領域の開始座標を更新する．
        /// </summary>
        private void StartPaint()
        {
            isPainting = true;
            //接触するinkUnitを探す
            InkUnitStorage.PaintInkUnit((Vector2)transform.position);
        }

        private void ContinuePaint()
        {
            //接触するinkUnitを探す
            InkUnitStorage.PaintInkUnit((Vector2)transform.position);
        }

        /// <summary>
        /// 塗り領域の終了座標を更新し，すべてのエリアに自身が塗られたか判定させる．
        /// </summary>
        private void EndPaint()
        {
            isPainting = false;
            paintAreaStorage.UpdateAreas();
        }
    }   
}