using Assets.Scripts.Stage;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerPaint : MonoBehaviour
    {
        [SerializeField] private PaintAreaStorage paintTargetAreaStorage; //すべてのエリアを管理
        private bool isPainting = false; //塗り中かどうか
        private Vector2 paintPosStart; //塗り領域の開始位置
        
        /// <summary>
        /// クリックしていたら塗る．
        /// </summary>
        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0) && !isPainting)
                StartPaint();
            if (!Input.GetMouseButton(0) && isPainting)
                EndPaint();
        }

        /// <summary>
        /// 塗り領域の開始座標を更新する．
        /// </summary>
        private void StartPaint()
        {
            isPainting = true;
            paintPosStart = transform.position;
        }

        /// <summary>
        /// 塗り領域の終了座標を更新し，すべてのエリアに自身が塗られたか判定させる．
        /// </summary>
        private void EndPaint()
        {
            isPainting = false;
            paintTargetAreaStorage.UpdateAreas(paintPosStart, transform.position);
        }
    }   
}