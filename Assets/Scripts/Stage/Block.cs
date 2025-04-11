using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Stage
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private InkUnit inkUnitPrefab;
        private readonly InkUnit[] inkUnits = new InkUnit[4];
        [SerializeField] private float inkUnitDepth = 0.1f; //インクユニットの深さ

        /// <summary>
        /// ブロックの各辺上のインクユニットを生成し，2次元配列に登録する．
        /// </summary>
        private void Start()
        {
            InstantiateInkUnits(0,Vector2.up);
            InstantiateInkUnits(1,Vector2.down);
            InstantiateInkUnits(2,Vector2.right);
            InstantiateInkUnits(3,Vector2.left);
        }

        /// <summary>
        /// ブロックの辺上に複数のインクユニットを生成する．
        /// </summary>
        private void InstantiateInkUnits(int arrayNumber, Vector2 edgeLocation)
        {
                InkUnit inkUnitInstanceNew = Instantiate(inkUnitPrefab, 
                new Vector2(transform.position.x + (transform.localScale.x / 2f + inkUnitDepth / 2f) * edgeLocation.x,
                transform.position.y + (transform.localScale.y / 2f + inkUnitDepth / 2f) * edgeLocation.y),
                Quaternion.identity);
                inkUnitInstanceNew.transform.localScale
                = new Vector2(inkUnitDepth * math.abs(edgeLocation.x) + transform.localScale.x * math.abs(edgeLocation.y),
                inkUnitDepth * math.abs(edgeLocation.y) + transform.localScale.y * math.abs(edgeLocation.x));
                inkUnits[arrayNumber] = inkUnitInstanceNew;
        }
    }
}
