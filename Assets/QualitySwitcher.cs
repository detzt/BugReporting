using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


/// <summary>
/// Switches between quality levels.
/// </summary>
public class QualitySwitcher : MonoBehaviour {
    [SerializeField, Tooltip("Reference to the quality selection dropdown")]
    private TMP_Dropdown qualitySelection = default;

    protected void Awake() {
        Assert.IsNotNull(qualitySelection);
        qualitySelection.options = QualitySettings.names.Select(name => new TMP_Dropdown.OptionData(name)).ToList();
        qualitySelection.onValueChanged.AddListener(value => StartCoroutine(SetQualityLevel(value)));
        qualitySelection.value = QualitySettings.GetQualityLevel();
    }

    public static IEnumerator SetQualityLevel(int index) {
        Debug.Log($"Setting quality level to {index} ({QualitySettings.names[index]})");

        // Apply new quality settings
        QualitySettings.SetQualityLevel(index, applyExpensiveChanges: true);

        // Here occurs the issue: There remains a leftover, but invalid volume stack from the previous quality level.

        while(!VolumeManager.instance.isInitialized)
            yield return null;

        Camera.main.UpdateVolumeStack();
    }



    /// <summary>
    /// This is the fixed version of <see cref="UniversalRenderPipeline.UpdateVolumeFramework"/>
    /// Specifically, this snippet was added:
    /// <code>
    /// // If an invalid volume stack is present, destroy it
    /// if (additionalCameraData.volumeStack != null && !additionalCameraData.volumeStack.isValid)
    /// {
    ///    camera.DestroyVolumeStack(additionalCameraData);
    /// }
    /// </summary>
    /*
    static void UpdateVolumeFramework(Camera camera, UniversalAdditionalCameraData additionalCameraData)
    {
        using var profScope = new ProfilingScope(ProfilingSampler.Get(URPProfileId.UpdateVolumeFramework));

        // We update the volume framework for:
        // * All cameras in the editor when not in playmode
        // * scene cameras
        // * cameras with update mode set to EveryFrame
        // * cameras with update mode set to UsePipelineSettings and the URP Asset set to EveryFrame
        bool shouldUpdate = camera.cameraType == CameraType.SceneView;
        shouldUpdate |= additionalCameraData != null && additionalCameraData.requiresVolumeFrameworkUpdate;

#if UNITY_EDITOR
        shouldUpdate |= Application.isPlaying == false;
#endif

        // When we have volume updates per-frame disabled...
        if (!shouldUpdate && additionalCameraData)
        {
            // If an invalid volume stack is present, destroy it
            if (additionalCameraData.volumeStack != null && !additionalCameraData.volumeStack.isValid)
            {
                camera.DestroyVolumeStack(additionalCameraData);
            }

            // Create a local volume stack and cache the state if it's null
            if (additionalCameraData.volumeStack == null)
            {
                camera.UpdateVolumeStack(additionalCameraData);
            }

            VolumeManager.instance.stack = additionalCameraData.volumeStack;
            return;
        }

        // When we want to update the volumes every frame...

        // We destroy the volumeStack in the additional camera data, if present, to make sure
        // it gets recreated and initialized if the update mode gets later changed to ViaScripting...
        if (additionalCameraData && additionalCameraData.volumeStack != null)
        {
            camera.DestroyVolumeStack(additionalCameraData);
        }

        // Get the mask + trigger and update the stack
        camera.GetVolumeLayerMaskAndTrigger(additionalCameraData, out LayerMask layerMask, out Transform trigger);
        VolumeManager.instance.ResetMainStack();
        VolumeManager.instance.Update(trigger, layerMask);
    }
    */

    /// <summary>
    /// While fixing this I also noticed a mistake in <see cref="VolumeManager.Deinitialize"/>
    /// The foreach loop does not use the loop variable and instead disposes the <code>stack</code> field.
    /// </summary>
    /*
    public void Deinitialize()
    {
        Debug.Assert(isInitialized);
        DestroyStack(m_DefaultStack);
        m_DefaultStack = null;
        foreach (var s in m_CreatedVolumeStacks)
            stack.Dispose(); // <-- This should be s.Dispose();
        m_CreatedVolumeStacks.Clear();
        baseComponentTypeArray = null;
        globalDefaultProfile = null;
        qualityDefaultProfile = null;
        customDefaultProfiles = null;
    }
    */
}
