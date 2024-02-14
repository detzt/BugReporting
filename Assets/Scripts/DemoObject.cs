using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Poolable object for demonstration purposes.
/// </summary>
public class DemoObject : MonoBehaviour, IPoolable
{
    private Pool<DemoObject> pool;

    public void Despawn()
    {
        gameObject.SetActive(false);
        pool.Release(this);
    }

    public void Spawn<TComp>(Vector3 pos, Quaternion rot, Pool<TComp> pool) where TComp : Component, IPoolable
    {
        Assert.IsTrue(pool is Pool<DemoObject>);

        this.pool = pool as Pool<DemoObject>;
        transform.position = pos;
        transform.rotation = rot;
        gameObject.SetActive(true);
    }
}
