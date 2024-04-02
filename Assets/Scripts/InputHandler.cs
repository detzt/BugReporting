using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, Controls.IPlayerActions
{
    private const float speed = 5f;

    [SerializeField, Tooltip("The object to move")]
    private Transform objectToMove;

    /// <summary>The generated C# class from the InputActionAsset</summary>
    private Controls controls;

    private Vector2 currentInput;


    protected void Awake()
    {
        Assert.IsNotNull(objectToMove);

        controls ??= new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    /// <summary>Dispose SerializedObjects before this <see cref="GameObject"/> gets destroyed</summary>
    protected void OnDestroy() {
        controls.Player.RemoveCallbacks(this);
        controls.Player.Disable();
        controls.Dispose();
    }


    protected void Update()
    {
        objectToMove.Translate(currentInput * speed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        if(context.phase == InputActionPhase.Performed)
        {
            currentInput = value;
        }
        else
        {
            currentInput = Vector2.zero;
        }
    }


    public void OnLook(InputAction.CallbackContext context) { }
    public void OnAttack(InputAction.CallbackContext context) { }
    public void OnInteract(InputAction.CallbackContext context) { }
    public void OnCrouch(InputAction.CallbackContext context) { }
    public void OnJump(InputAction.CallbackContext context) { }
    public void OnPrevious(InputAction.CallbackContext context) { }
    public void OnNext(InputAction.CallbackContext context) { }
    public void OnSprint(InputAction.CallbackContext context) { }
}
