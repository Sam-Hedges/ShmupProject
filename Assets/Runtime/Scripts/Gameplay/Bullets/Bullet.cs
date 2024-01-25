using UnityEngine;


public class Bullet : MonoBehaviour
{
    internal BulletPoolManager manager;
    private Vector3 direction;
    private float speed;
    [SerializeField] private float damage = 100f;
    
    private void OnCollisionEnter(Collision col)
    {
        Debug.Log($"Bullet collided with {col.gameObject}");
        if (col.gameObject.layer == LayerMask.NameToLayer($"Player")) { return; }
        Enemy enemy = col.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.Damage(damage);
        }
        manager.DisableBullet(this);
    }

    public void Fire(Vector3 dir, float spd, BulletPoolManager mngr)
    {
        direction = dir; 
        speed = spd;
        manager = mngr;
    }
    
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}