using UnityEngine;


namespace Assets.Scripts.Stage
{
    public class InkUnit : MonoBehaviour
    {
        /// <summary>
        /// ストレージに登録する．
        /// </summary>
        private void Awake()
        {
            InkUnitStorage.AddInkUnit(this);   
        }

        /// <summary>
        /// プレイヤーによって塗られたか判定する．
        /// </summary>
        public bool IsPainted(Vector2 playerPaintPos)
        {
            Vector2 inkUnitPosMin = transform.position - transform.localScale / 2f;
            Vector2 inkUnitPosMax = transform.position + transform.localScale / 2f;
            return inkUnitPosMin.x <= playerPaintPos.x && playerPaintPos.x <= inkUnitPosMax.x
            && inkUnitPosMin.y <= playerPaintPos.y && playerPaintPos.y <= inkUnitPosMax.y;
        }

        /// <summary>
        /// スプライトを変更する．
        /// </summary>
        public void Paint()
        {
            GetComponent<SpriteRenderer>().color = Color.cyan; //変更予定．
        }

        /// <summary>
        /// スプライトを変更する．
        /// </summary>
        public void PaintError()
        {
            GetComponent<SpriteRenderer>().color = Color.red; //変更予定．
        }
    }
}

