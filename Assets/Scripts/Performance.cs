using UnityEngine;

/// <summary>
/// Framerate and performance utility class.
/// </summary>
public class Performance : MonoBehaviour {

    /// <summary>
    /// Sets the target framerate.<br/>
    /// A value of -1 means unlimited framerate on desktop and default framerate (30) on mobile.
    /// </summary>
    public static void SetTargetFrameRate(int value) {
        Application.targetFrameRate = value;
    }

    /// <summary>
    /// Returns the maximal refresh rate that the screen supports.
    /// </summary>
    public static int GetMaxScreenRefreshRate() {
        var maxRefreshRate = 0;
        foreach (var resolution in Screen.resolutions) {
            var refreshRate = (int)System.Math.Round(resolution.refreshRateRatio.value);
            if (refreshRate > maxRefreshRate) {
                maxRefreshRate = refreshRate;
            }
        }
        return maxRefreshRate;
    }
}
