using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

/// <summary>
/// Provides pooling of the configured object
/// </summary>
public class Pool<TComp> : IObjectPool<TComp> where TComp : Component, IPoolable
{
    private readonly GameObject prefab;
    private readonly Stack<TComp> pool;
    private readonly GameObject container;

    public int CountInactive => pool.Count;

    /// <summary>
    /// Creates a new pool for the given <paramref name="prefab"/>
    /// </summary>
    /// <param name="prefab">The prefab to pool</param>
    public Pool(GameObject prefab)
    {
        Assert.IsNotNull(prefab);
        Assert.IsNotNull(prefab.GetComponentInChildren<TComp>());
        pool = new Stack<TComp>();
        this.prefab = prefab;
        container = new GameObject("Pool");
        Object.DontDestroyOnLoad(container);
    }

    /// <summary>
    /// Recycles or creates an object and <see cref="IPoolable.Spawn"/>s it at the given location
    /// </summary>
    /// <param name="pos">The position to <see cref="IPoolable.Spawn"/> at</param>
    /// <param name="rot">The rotation to <see cref="IPoolable.Spawn"/> at</param>
    /// <returns>The <see cref="IPoolable.Spawn"/>ned <see cref="TComp"/></returns>
    public TComp Spawn(Vector3 pos, Quaternion rot)
    {
        TComp obj = pool.Count > 0 ? pool.Pop() : Object.Instantiate(prefab, container.transform).GetComponentInChildren<TComp>();
        obj.Spawn(pos, rot, this);
        return obj;
    }

    /// <summary>Adds the given <paramref name="obj"/> to the <see cref="pool"/> of inactive objects</summary>
    public void Release(TComp obj)
    {
        pool.Push(obj);
    }

    /// <summary>Clears the pool of all objects</summary>
    public void Clear()
    {
        foreach (Transform t in container.transform)
            Object.Destroy(t.gameObject);
        foreach (var obj in pool)
            Object.Destroy(obj.gameObject);
        pool.Clear();
    }

    /// <summary><see cref="Spawn(Vector3, Quaternion)"/>s a new instance with identity transform.</summary>
    public TComp Get() => Spawn(Vector3.zero, Quaternion.identity);

    /// <summary>
    /// Should be used in a <c>using</c> statement to return the object to the pool when done.
    /// But does not work due to inaccessible Unity internals.
    /// </summary>
    public PooledObject<TComp> Get(out TComp v)
    {
        // This would be the proper implementation, but the contents of PooledObject are inaccessible.
        // This constructor is internal:
        // return new PooledObject<T>(v = Get(), this);
        // And the fields are private:
        // return new PooledObject<TComp>() { m_ToReturn = v, m_Pool = this };

        // Withe the empty constructor (which should actually not exist), the object won't know which pool to return to when disposed.
        v = Get();
        return new PooledObject<TComp>();
    }
}
