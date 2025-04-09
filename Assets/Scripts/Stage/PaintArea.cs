using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Stage
{
    public class PaintArea : MonoBehaviour
    {
        [SerializeField] private float perfectWidth = 0.1f; //PARFECT許容幅
        private bool isActive = false; //有効かどうか
        private Vector2 posMin, posMax; //最小座標と最大座標

        /// <summary>
        /// 最小座標と最大座標を計算する．
        /// </summary>
        private void Awake()
        {
            posMin = (Vector2)transform.position - (Vector2)transform.localScale / 2f;
            posMax = (Vector2)transform.position + (Vector2)transform.localScale / 2f;
        }

        /// <summary>
        /// 有効化する．
        /// </summary>
        public void Activate()
        {
            isActive = true;
        }

        /// <summary>
        /// 塗り領域と接触しているか判定する．
        /// </summary>
        public bool IsColliding(Vector2 paintPosStart, Vector2 paintPosEnd)
        {
            //塗り領域の最小座標と最大座標を計算する．
            Vector2 paintPosMin = Vector2.Min(paintPosStart, paintPosEnd);
            Vector2 paintPosMax = Vector2.Max(paintPosStart, paintPosEnd);
            return isActive && IsOverlapping(paintPosMin, paintPosMax);
        }

        /// <summary>
        /// 塗り領域と重なっている(有効or無効は気にしない)か判定する．
        /// </summary>
        private bool IsOverlapping(Vector2 paintPosMin, Vector2 paintPosMax)
        {
            return paintPosMin.x < posMax.x && paintPosMax.x > posMin.x &&
            paintPosMin.y < posMax.y && paintPosMax.y > posMin.y;
        }

        /// <summary>
        /// 塗りを評価し，無効化する．
        /// </summary>
        private void Paint(Vector2 paintPosStart, Vector2 paintPosEnd)
        {
            Debug.Log(Evaluate(paintPosStart, paintPosEnd));
            GetComponent<Renderer>().material.color = Color.clear; //変更予定．
            isActive = false;
        }

        /// <summary>
        /// 塗り領域との接触領域によって評価する．
        /// </summary>
        private string Evaluate(Vector2 paintPosStart, Vector2 paintPosEnd)
        {
            //塗り領域の最小座標と最大座標を計算する．
            Vector2 paintPosMin = Vector2.Min(paintPosStart, paintPosEnd);
            Vector2 paintPosMax = Vector2.Max(paintPosStart, paintPosEnd);
            (Vector2 areaPosMinInside, Vector2 areaPosMaxInside) = CalculatePosInside();
            //塗り領域の端がPERFECT許容幅以内に存在する場合．
            if ((posMin.x <= paintPosMin.x && paintPosMin.x <= areaPosMinInside.x
            && areaPosMaxInside.x <= paintPosMax.x && paintPosMax.x <= posMax.x)
            || (posMin.y <= paintPosMin.y && paintPosMin.y <= areaPosMinInside.y
            && areaPosMaxInside.y <= paintPosMax.y && paintPosMax.y <= posMax.y))
                return "PERFECT";
            //塗り領域がエリアに収まっていない場合．
            if (paintPosMin.x < posMin.x || posMax.x < paintPosMax.x 
            || paintPosMin.y < posMin.y || posMax.y < paintPosMax.y)
                return "BAD";
            return "GOOD";
        }

        /// <summary>
        /// PERFECTにならない領域の最小座標と最大座標を計算する．
        /// </summary>
        private (Vector2,Vector2) CalculatePosInside()
        {
            if (posMax.y - posMin.y < posMax.x - posMin.x)
                return (new Vector2(posMin.x + perfectWidth, posMin.y),
                new Vector2(posMax.x - perfectWidth, posMax.y));
            return (new Vector2(posMin.x, posMin.y + perfectWidth),
            new Vector2(posMax.x, posMax.y - perfectWidth));
        }

        /// <summary>
        /// 塗り領域の開始位置からの距離を計算する．
        /// </summary>
        public float GetDistFromStart(Vector2 paintPosStart)
        {
            return Vector2.Distance(paintPosStart, transform.position);
        }

        /// <summary>
        /// 連鎖したエリアを塗る．
        /// </summary>
        public void PaintInChain(Vector2 paintPosStart, Vector2 paintPosEnd)
        {
            //塗りの向き．
            Vector2 paintDirection = paintPosEnd - paintPosStart;
            if(math.abs(paintDirection.x) > math.abs(paintDirection.y) && paintDirection.x > 0)
                Paint(paintPosStart, new Vector2(posMax.x, paintPosEnd.y));
            if(math.abs(paintDirection.x) > math.abs(paintDirection.y) && paintDirection.x < 0)
                Paint(paintPosStart, new Vector2(posMin.x, paintPosEnd.y));
            if(math.abs(paintDirection.x) < math.abs(paintDirection.y) && paintDirection.y > 0)
                Paint(paintPosStart, new Vector2(paintPosEnd.x, posMax.y));
            if(math.abs(paintDirection.x) < math.abs(paintDirection.y) && paintDirection.y < 0)
                Paint(paintPosStart, new Vector2(paintPosEnd.x, posMin.y));
        }

        /// <summary>
        /// エリアの端の座標を返す．
        /// </summary>
        public Vector2 GetEdgePos(Vector2 paintPosStart, Vector2 paintPosEnd)
        {
            //塗りの向き．
            Vector2 paintDirection = paintPosEnd - paintPosStart;
            //塗りの向きに応じた端の座標(塗りながらエリア外に出ていくときに交差する点の座標)を返す．
            if (math.abs(paintDirection.x) > math.abs(paintDirection.y) && paintDirection.x > 0)
                return new Vector2(posMax.x, paintPosStart.y);
            if (math.abs(paintDirection.x) > math.abs(paintDirection.y) && paintDirection.x < 0)
                return new Vector2(posMin.x, paintPosStart.y);
            if (math.abs(paintDirection.x) < math.abs(paintDirection.y) && paintDirection.y > 0)
                return new Vector2(paintPosStart.x, posMax.y);
            if (math.abs(paintDirection.x) < math.abs(paintDirection.y) && paintDirection.y < 0)    
                return new Vector2(paintPosStart.x, posMin.y);
            return paintPosStart;
        }

        /// <summary>
        /// 連鎖の最後のエリアを塗る．
        /// </summary>
        public void PaintOnEndOfChain(Vector2 paintPosStart, Vector2 paintPosEnd)
        {
            Paint(paintPosStart, paintPosEnd);
        }
    }
}