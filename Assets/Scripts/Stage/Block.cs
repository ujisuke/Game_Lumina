using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Stage
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private GameObject colorUnitPrefab;
        private const int unitNumber = 5;
        private GameObject[,] colorUnits = new GameObject[4,unitNumber];
        [SerializeField] private float colorUnitDepth = 0.1f;

        private void Start()
        {
            InstantiateColorUnit(0);
        }

        private void InstantiateColorUnit(int arrayNumber)
        {
            for(int i = 0; i < unitNumber; i++)
            {
                colorUnits[arrayNumber, i] = Instantiate(colorUnitPrefab, 
                (Vector2)transform.position + new Vector2(-transform.localScale.x / 2f + transform.localScale.x / unitNumber * (i + 1 / 2f), 
                transform.localScale.y / 2f - colorUnitDepth / 2f), Quaternion.identity);
                colorUnits[arrayNumber, i].transform.localScale = new Vector2(transform.localScale.x / unitNumber, colorUnitDepth);
            }
        }
    }
}
