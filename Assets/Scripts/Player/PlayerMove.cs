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
        //壁とか床のレイヤー番号
        private readonly int wallLayerNumber = 1 << 6;
        //クリックしているか否か
        //後々削除する予定
        private bool isClicking = false;
        //攻撃後か否か
        private bool isAfterAttack = false;
        //攻撃後の移動の初速度
        [SerializeField] private float moveSpeedAfterAttack = 11f;
        //攻撃後の横移動の速度
        private float moveSpeedAfterAttackHorizontally = 0f;

        private void FixedUpdate()
        {
            //攻撃の入力
            //後々削除する予定
            if (Input.GetKey(KeyCode.Mouse0) && !isClicking)
            {
                isClicking = true;
                Attack();
            }
            if(!Input.GetKey(KeyCode.Mouse0))
                isClicking = false;

            MoveHorizontally();
            MoveVertically();

            if(IsOnFloor())
                isAfterAttack = false;
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
            AdjustPosX();
        }

        //移動後のX座標の修正
        private void AdjustPosX()
        {
            if(IsOnRightWall())
                AdjustPosXOnRightWall();
            if(IsOnLeftWall())
                AdjustPosXOnLeftWall();
        }

        //右の壁と接触しているかどうか
        private bool IsOnRightWall()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.x / 2f * Vector3.right, Vector2.right);
            return Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider != null;
        }

        //移動後のX座標の修正(右の壁)
        private void AdjustPosXOnRightWall()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.x / 2f * Vector3.right, Vector2.right);
            Collider2D wallCollider = Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider;
            float adjustedPosX = wallCollider.transform.position.x - wallCollider.transform.localScale.x / 2f - playerCollider.transform.localScale.x / 2f;
            transform.position = new Vector2(adjustedPosX, transform.position.y);
        }

        //左の壁と接触しているかどうか
        private bool IsOnLeftWall()
        {
            Ray2D ray = new(playerCollider.transform.position - playerCollider.transform.localScale.x / 2f * Vector3.right, Vector2.left);
            return Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider != null;
        }

        //移動後のX座標の修正(左の壁)
        private void AdjustPosXOnLeftWall()
        {
            Ray2D ray = new(playerCollider.transform.position - playerCollider.transform.localScale.x / 2f * Vector3.right, Vector2.left);
            Collider2D wallCollider = Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider;
            float adjustedPosX = wallCollider.transform.position.x + wallCollider.transform.localScale.x / 2f + playerCollider.transform.localScale.x / 2f;
            transform.position = new Vector2(adjustedPosX, transform.position.y);
        }

        //縦移動
        private void MoveVertically()
        {
            if(Input.GetKey(KeyCode.Space) && IsOnFloor())
                moveSpeedVerticallyPrev = jumpSpeedFirst;
            transform.position += (moveSpeedVerticallyPrev - landingSpeed) * Time.deltaTime * Vector3.up;
            if(IsOnFloor())
                moveSpeedVerticallyPrev = 0f;
            else
                moveSpeedVerticallyPrev -= landingSpeed;
            AdjustPosY();
        }

        //床と接触しているかどうか
        private bool IsOnFloor()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.y / 2f * Vector3.down, Vector2.down);
            return Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider != null;
        }

        //移動後のY座標の修正
        private void AdjustPosY()
        {
            if(IsOnFloor())
                AdjustPosYOnFloor();
            if(IsOnCeiling())
                AdjustPosYOnCeiling();
        }

        //移動後のY座標の修正(床)
        private void AdjustPosYOnFloor()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.y / 2f * Vector3.down, Vector2.down);
            Collider2D floorCollider = Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider;
            float adjustedPosY = floorCollider.transform.position.y + floorCollider.transform.localScale.y / 2f + playerCollider.transform.localScale.y / 2f;
            transform.position = new Vector2(transform.position.x, adjustedPosY);
        }

        //天井と接触しているかどうか
        private bool IsOnCeiling()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.y / 2f * Vector3.up, Vector2.up);
            return Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider != null;
        }

        //移動後のY座標の修正(天井)
        private void AdjustPosYOnCeiling()
        {
            Ray2D ray = new(playerCollider.transform.position + playerCollider.transform.localScale.y / 2f * Vector3.up, Vector2.up);
            Collider2D ceilingCollider = Physics2D.Raycast(ray.origin, ray.direction, 0f, wallLayerNumber).collider;
            float adjustedPosY = ceilingCollider.transform.position.y - ceilingCollider.transform.localScale.y / 2f - playerCollider.transform.localScale.y / 2f;
            transform.position = new Vector2(transform.position.x, adjustedPosY);
        }

        //攻撃の反動による移動
        //後々別のクラスから呼び出す予定
        public void Attack()
        {
            Vector2 blownDirection = ((Vector2)(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition))).normalized;
            Debug.Log(blownDirection);
            moveSpeedVerticallyPrev = moveSpeedAfterAttack * blownDirection.y;
            isAfterAttack = true;
            moveSpeedAfterAttackHorizontally = moveSpeedAfterAttack * blownDirection.x;
        }
    }
}