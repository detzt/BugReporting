using UnityEngine;

/// <summary>
/// Interface for everything that can be pooled in a <see cref="Pool{TComp}"/>
/// </summary>
public interface IPoolable
{

    /// <summary>
    /// Activates this object and places it at the given location
    /// </summary>
    /// <param name="pos">The position to spawn at</param>
    /// <param name="rot">The rotation to spawn at</param>
    /// <param name="pool">The pool to return to when despawned</param>
    void Spawn<TComp>(Vector3 pos, Quaternion rot, Pool<TComp> pool) where TComp : Component, IPoolable;

    /// <summary>
    /// Deactivates this object and returns it to its <see cref="Pool{TComp}"/>.<br/>
    /// Usually implemented like this:
    /// <example>
    /// <code>
    /// public void Despawn()
    /// {
    ///    gameObject.SetActive(false);
    ///    pool.Release(this);
    /// }
    /// </code>
    /// </example>
    /// </summary>
    void Despawn();
}
