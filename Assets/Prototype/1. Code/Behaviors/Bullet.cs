using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Public Variables 
    public Vector2 BulletDirection { get => bulletDirection; set => bulletDirection = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Vector2 bulletDirection;
    [SerializeField] private float _bulletForce;
    [SerializeField] private float _bulletForceUp;
    [SerializeField] private float _autoDestroyTime;
    private Rigidbody2D _bulletBody;
    #endregion

    #region Unity Methods 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bulletBody = GetComponent<Rigidbody2D>();
        BulletMove();
    }

    // Update is called once per frame
    void Update()
    {
        AutoDestroy();
    }

    private void FixedUpdate()
    {

    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    private void BulletMove()
    {
        _bulletBody.linearVelocity = bulletDirection * bulletSpeed;
    }

    public void SetDirection(Vector2 pDir)
    {
        bulletDirection = pDir.normalized;
    }

    private void AutoDestroy()
    {
        _autoDestroyTime -= Time.deltaTime;

        if (_autoDestroyTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<DummyEnemy>().Knockback(transform,_bulletForce, _bulletForceUp);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Untagged")
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
