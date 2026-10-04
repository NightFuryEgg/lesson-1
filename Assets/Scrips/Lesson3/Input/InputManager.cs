using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private const string BindingSaveKey = "RollBindingPath";

    private const string DefaultBindingPath = "<Keyboard>/space";

    private InputAction rollAction;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    private bool isRebinding = false;

    private void Awake()
    {

        rollAction = new InputAction("Roll", InputActionType.Button, DefaultBindingPath);


        LoadBinding();
    }

    private void OnEnable()
    {

        rollAction.Enable();
    }

    private void OnDisable()
    {
        rollAction.Disable();
    }

    private void OnDestroy()
    {

        if (rebindOperation != null)
        {
            rebindOperation.Dispose();
        }
        rollAction.Dispose();
    }

    private void Update()
    {

        if (rollAction.WasPressedThisFrame())
        {
            gameManager.RollDice();
        }
    }


    public string GetRollKeyName()
    {
        return rollAction.GetBindingDisplayString();
    }


    public bool IsRebinding()
    {
        return isRebinding;
    }


    public void StartRebinding()
    {
        if (isRebinding)
        {
            return;
        }

        isRebinding = true;


        rollAction.Disable();

        rebindOperation = rollAction.PerformInteractiveRebinding(0)
            .WithControlsHavingToMatchPath("<Keyboard>")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(OnRebindFinished)
            .OnCancel(OnRebindFinished)
            .Start();
    }


    private void OnRebindFinished(InputActionRebindingExtensions.RebindingOperation operation)
    {
        operation.Dispose();
        rebindOperation = null;
        isRebinding = false;

        SaveBinding();
        rollAction.Enable(); 
    }


    public void ResetBinding()
    {
        if (isRebinding)
        {
            return;
        }

        rollAction.RemoveAllBindingOverrides(); 
        SaveBinding();
    }


    private void SaveBinding()
    {
        string overridePath = rollAction.bindings[0].overridePath;

        if (string.IsNullOrEmpty(overridePath))
        {
            PlayerPrefs.DeleteKey(BindingSaveKey); 
        }
        else
        {
            PlayerPrefs.SetString(BindingSaveKey, overridePath);
        }

        PlayerPrefs.Save();
    }

    private void LoadBinding()
    {
        string savedPath = PlayerPrefs.GetString(BindingSaveKey, "");

        if (savedPath != "")
        {
            rollAction.ApplyBindingOverride(0, savedPath);
        }
    }
}