using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private PlayerMove playerMove;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private GameObject bulletEffectPrefab;
        private readonly int wallLayerNumber = 1 << 6;
        //クリックしているか否か
        private bool isClicking = false;

        private void FixedUpdate()
        {
            //攻撃の入力
            if (Input.GetKey(KeyCode.Mouse0) && !isClicking)
                Attack();
            if(!Input.GetKey(KeyCode.Mouse0))
                isClicking = false;
        }

        private async void Attack()
        {
            isClicking = true;
            playerMove.Attack();
            Vector2 attackDirection = ((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position)).normalized;
            Ray2D ray = new(transform.position, attackDirection);
            Vector2 hitPos = Physics2D.Raycast(ray.origin, ray.direction, wallLayerNumber).point;
            float distance = Vector2.Distance(ray.origin, hitPos);
            float waitTime = distance / bulletSpeed;
            var newBulletEffectPrefab = Instantiate(bulletEffectPrefab, hitPos, Quaternion.identity);
            for(int i = 1; i < 11; i++)
            {
                newBulletEffectPrefab.transform.position = Vector2.Lerp(ray.origin, hitPos, i / 10f);
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime / 10f));
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            Destroy(newBulletEffectPrefab);
        }        
    }
}
