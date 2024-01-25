using UnityEngine.Pool;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] private Enemy prefab;
    [SerializeField] private int defaultCapacity = 100;
    [SerializeField] private int maxCapacity = 1000;
    
    private ObjectPool<Enemy> pool;
    
    private void Awake()
    {
        pool = new ObjectPool<Enemy>(() => { return Instantiate(prefab); }, 
            enemy => { enemy.gameObject.SetActive(true); }, 
            enemy => { enemy.gameObject.SetActive(false); },
            enemy => { Destroy(enemy.gameObject); }, false,
            defaultCapacity, maxCapacity);
    }
    
    public Enemy RequestEnemy()
    {
        return pool.Get();
    }
 
    public void DisableEnemy(Enemy enemy)
    {
        pool.Release(enemy);
    }
}