using UnityEngine;


namespace Assets.Scripts.Stage
{
    public class Ink : MonoBehaviour
    {
        public void DryOnTargetArea(Vector2 areaPosMin, Vector2 areaPosMax)
        {
            var errorInkPrefab = Instantiate(this, transform.position, Quaternion.identity);
            errorInkPrefab.GetComponent<SpriteRenderer>().color = Color.red;
            //transform.localPosition = 
            //new Vector3(transform.localPosition.x, transform.localPosition.y, PaintTargetAreaStorage.inkPosZ + 1);
            
            Vector2 inkPosMin = transform.position - transform.localScale / 2f;
            Vector2 errorInkPosMax = transform.position + transform.localScale / 2f;
            Vector2 validInkPosMin = Vector2.Max(inkPosMin, areaPosMin);
            Vector2 validInkPosMax = Vector2.Min(errorInkPosMax, areaPosMax);
            transform.position = (validInkPosMin + validInkPosMax) / 2f;
            transform.localScale = validInkPosMax - validInkPosMin;
        }

        public void DryNotOnTargetArea()
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            //transform.localPosition = 
            //new Vector3(transform.localPosition.x, transform.localPosition.y, PaintTargetAreaStorage.inkPosZ + 1);
            //Debug.Log("BAD");
        }
    }
}

