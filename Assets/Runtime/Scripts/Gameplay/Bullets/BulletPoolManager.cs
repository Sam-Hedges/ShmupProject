using UnityEngine.Pool;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int defaultCapacity = 100;
    [SerializeField] private int maxCapacity = 1000;
    
    private ObjectPool<Bullet> pool;
    
    private void Start()
    {
        pool = new ObjectPool<Bullet>(() => { return Instantiate(bulletPrefab); }, 
            bullet => { bullet.gameObject.SetActive(true); }, 
            bullet => { bullet.gameObject.SetActive(false); },
            bullet => { Destroy(bullet.gameObject); }, false,
            defaultCapacity, maxCapacity);
    }
    
    public Bullet RequestBullet()
    {
        return pool.Get();
    }

    public void DisableBullet(Bullet bullet)
    {
        pool.Release(bullet);
    }
}