using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// Input processor to rotate a two dimensional input by the configured degrees
/// </summary>
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class RotateProcessor : InputProcessor<Vector2>
{
    [Tooltip("The angle in degrees to rotate the vector by")]
    public float RotateDegrees = 45f;

#if UNITY_EDITOR
    static RotateProcessor()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        InputSystem.RegisterProcessor<RotateProcessor>();
    }

    /// <summary>
    /// Rotates the given vector <paramref name="value"/> by the configured <see cref="RotateDegrees"/>
    /// </summary>
    public override Vector2 Process(Vector2 value, InputControl control) => Rotate(value, RotateDegrees);

    /// <summary>
    /// Rotates the given vector <paramref name="v"/> by the given <paramref name="degrees"/>
    /// </summary>
    /// <param name="v">The vector to rotate</param>
    /// <param name="degrees">The degrees to rotate the vector by</param>
    /// <returns>The rotated vector</returns>
    private static Vector2 Rotate(Vector2 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = cos * tx - sin * ty;
        v.y = sin * tx + cos * ty;
        return v;
    }
}
