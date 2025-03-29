using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float moveSpeedHorizontally = 9.7f;
        [SerializeField] private float jumpSpeedFirst = 0.25f;
        [SerializeField] private float landingSpeed = 0.02f;
        [SerializeField] private Collider2D playerCollider;
        private float speedVerticallyPrev = 0f;
        private readonly int wallLayerNumber = 1 << 6; 

        private void FixedUpdate()
        {
            MoveHorizontally();
            MoveVertically();

            Debug.Log(IsOnCeiling() + " " + IsOnFloor() + " " + IsOnLeftWall() + " " + IsOnRightWall());
        }
        
        private void MoveHorizontally()
        {
            if(Input.GetKey(KeyCode.D))
                transform.position += moveSpeedHorizontally * Time.deltaTime * Vector3.right;
            if(Input.GetKey(KeyCode.A))
                transform.position += moveSpeedHorizontally * Time.deltaTime * Vector3.left;
            AdjustPosX();
        }

        private void AdjustPosX()
        {
            if(IsOnRightWall())
                AdjustPosXOnRightWall();
            if(IsOnLeftWall())
                AdjustPosXOnLeftWall();
        }

        private bool IsOnRightWall()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.x / 2f * Vector3.right, Vector2.right);
            return Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider != null;
        }

        private void AdjustPosXOnRightWall()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.x / 2f * Vector3.right, Vector2.right);
            Collider2D wallCollider = Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider;
            float adjustedPosX = wallCollider.transform.position.x - wallCollider.transform.localScale.x / 2f - playerCollider.transform.localScale.x / 2f;
            transform.position = new Vector2(adjustedPosX, transform.position.y);
        }

        private bool IsOnLeftWall()
        {
            Ray2D ray = new(playerCollider.transform.position - playerCollider.transform.localScale.x / 2f * Vector3.right, Vector2.left);
            return Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider != null;
        }

        private void AdjustPosXOnLeftWall()
        {
            Ray2D ray = new(playerCollider.transform.position - playerCollider.transform.localScale.x / 2f * Vector3.right, Vector2.left);
            Collider2D wallCollider = Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider;
            float adjustedPosX = wallCollider.transform.position.x + wallCollider.transform.localScale.x / 2f + playerCollider.transform.localScale.x / 2f;
            transform.position = new Vector2(adjustedPosX, transform.position.y);
        }

        private void MoveVertically()
        {
            if(Input.GetKey(KeyCode.Space) && IsOnFloor())
                speedVerticallyPrev = jumpSpeedFirst;
            transform.position += (speedVerticallyPrev - landingSpeed) * Vector3.up;
            if(IsOnFloor())
                speedVerticallyPrev = 0f;
            else
                speedVerticallyPrev -= landingSpeed;
            AdjustPosY();
        }

        private bool IsOnFloor()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.y / 2f * Vector3.down, Vector2.down);
            return Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider != null;
        }

        private void AdjustPosY()
        {
            if(IsOnFloor())
                AdjustPosYOnFloor();
            if(IsOnCeiling())
                AdjustPosYOnCeiling();
        }

        private void AdjustPosYOnFloor()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.y / 2f * Vector3.down, Vector2.down);
            Collider2D floorCollider = Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider;
            float adjustedPosY = floorCollider.transform.position.y + floorCollider.transform.localScale.y / 2f + playerCollider.transform.localScale.y / 2f;
            transform.position = new Vector2(transform.position.x, adjustedPosY);
        }

        private bool IsOnCeiling()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.y / 2f * Vector3.up, Vector2.up);
            return Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider != null;
        }

        private void AdjustPosYOnCeiling()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.y / 2f * Vector3.up, Vector2.up);
            Collider2D ceilingCollider = Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider;
            float adjustedPosY = ceilingCollider.transform.position.y - ceilingCollider.transform.localScale.y / 2f - playerCollider.transform.localScale.y / 2f;
            transform.position = new Vector2(transform.position.x, adjustedPosY);
        }
    }
}