using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMove : MonoBehaviour
    {
        //横移動速度
        [SerializeField] private float moveSpeedHorizontally = 9.7f;
        //ジャンプの初速度
        [SerializeField] private float jumpSpeedFirst = 6f;
        //ジャンプの減速
        [SerializeField] private float landingSpeed = 0.4f;
        //プレイヤーが持つコライダー
        [SerializeField] private Collider2D playerCollider;
        //直前の縦移動速度
        private float moveSpeedVerticallyPrev = 0f;
        //壁とか床のレイヤーマスク
        private readonly int wallLayerMask = 1 << 6;
        //攻撃後か否か
        private bool isAfterAttack = false;
        //攻撃後の移動の初速度
        [SerializeField] private float moveSpeedAfterAttack = 11f;
        //攻撃後の横移動の速度
        private float moveSpeedAfterAttackHorizontally = 0f;
        //直前の位置
        Vector2 posPrev = Vector2.zero;

        private void FixedUpdate()
        {
            MoveHorizontally();
            MoveVertically();

            if(IsOnSurface(Vector2.down))
                isAfterAttack = false;
            posPrev = playerCollider.transform.position;
        }
        
        //横移動
        private void MoveHorizontally()
        {
            if(Input.GetKey(KeyCode.D) && !isAfterAttack)
                transform.position += moveSpeedHorizontally * Time.deltaTime * Vector3.right;
            if(Input.GetKey(KeyCode.A) && !isAfterAttack)
                transform.position += moveSpeedHorizontally * Time.deltaTime * Vector3.left;
            if(isAfterAttack)
                transform.position += moveSpeedAfterAttackHorizontally * Time.deltaTime * Vector3.right;
            AdjustPos(Vector2.right);
            AdjustPos(Vector2.left);
        }

        private bool IsOnSurface(Vector2 direction)
        {
            Vector2 colliderEdgePos = (Vector2)transform.position + Vector2.Scale(playerCollider.transform.localScale / 2f, direction);
            Ray2D ray = new(colliderEdgePos, direction);
            return Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerMask).collider != null;
        }

        //縦移動
        private void MoveVertically()
        {
            if(Input.GetKey(KeyCode.Space) && IsOnSurface(Vector2.down))
                moveSpeedVerticallyPrev = jumpSpeedFirst;
            transform.position += (moveSpeedVerticallyPrev - landingSpeed) * Time.deltaTime * Vector3.up;
            if(IsOnSurface(Vector2.down))
                moveSpeedVerticallyPrev = 0f;
            else
                moveSpeedVerticallyPrev -= landingSpeed;
            AdjustPos(Vector2.up);
            AdjustPos(Vector2.down);
        }

        //壁にめり込んだり貫通するのを防ぐために位置を調整する
        private void AdjustPos(Vector2 direction)
        {
            Vector2 directionUnit = new(math.abs(direction.x), math.abs(direction.y));
            Vector2 directionNormalUnit = new(math.abs(direction.y), math.abs(direction.x));
            Vector2 path = Vector2.Scale((Vector2)playerCollider.transform.position - posPrev, directionUnit);
            Vector2 offset = Vector2.Scale(playerCollider.transform.localScale / 2f, direction);
            Ray2D ray = new(posPrev + offset, path);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, path.magnitude, wallLayerMask);
            if(hit.collider != null && Vector2.Dot(path, direction) > 0f)
                transform.position = Vector2.Scale(hit.point - offset, directionUnit) + Vector2.Scale(playerCollider.transform.position, directionNormalUnit);
        }

        //攻撃の反動による移動
        public void Attack()
        {
            Vector2 blownDirection = ((Vector2)(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition))).normalized;
            moveSpeedVerticallyPrev = moveSpeedAfterAttack * blownDirection.y;
            isAfterAttack = true;
            moveSpeedAfterAttackHorizontally = moveSpeedAfterAttack * blownDirection.x;
        }
    }
}