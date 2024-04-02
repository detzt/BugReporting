using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Sets up functionality of the debug menu.
/// </summary>
public class DebugMenuBinding : MonoBehaviour {

    protected void OnEnable() {
        var uiDocument = GetComponent<UIDocument>();
        RegisterBindings(uiDocument.rootVisualElement);
    }

    public void RegisterBindings(VisualElement element) {

        // Set target framerate
        var targetFramerateField = element.Q<SliderInt>("TargetFPS");
        targetFramerateField.highValue = Performance.GetMaxScreenRefreshRate();
        _ = targetFramerateField.RegisterValueChangedCallback(evt => {
            Performance.SetTargetFrameRate(evt.newValue);
        });

        // Show the unmodified default value in the slider
        targetFramerateField.SetValueWithoutNotify(Application.targetFrameRate);
    }
}
