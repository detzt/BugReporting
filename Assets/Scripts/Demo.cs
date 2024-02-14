using UnityEngine;

/// <summary>
/// Demonstrates the issue.
/// </summary>
public class Demo : MonoBehaviour
{
    [SerializeField, Tooltip("The prefab to pool")]
    private GameObject prefab;

    private Pool<DemoObject> pool;

    protected void Awake()
    {
        pool = new Pool<DemoObject>(prefab);
    }

    protected void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            using (pool.Get(out var demoObject))
            {
                demoObject.name = $"DemoObject {i}";
            }
        }
    }
}
