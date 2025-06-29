using System.Collections.Generic;
using UnityEngine;

// IInteractable interface means that whoever is implementing this interface MUST
// have a function of Interact (the abstract function in my IInteractable interface)
public class LightSwitchView : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Light> lightsources = new List<Light>();
    private SwitchState currentState;

    public delegate void LightSwitchDelegate(); // The signature of my delegate
    public LightSwitchDelegate lightSwitch; // An instance of my signature

    private void Start() => currentState = SwitchState.Off;

    private void OnEnable()
    {
        // Always better to assign instances in onEnable
        lightSwitch = onLightSwitchToggled;
    }

    public void Interact()
    {
        // Now to invoke our delegate
        lightSwitch.Invoke();
    }

    private void onLightSwitchToggled()
    {
        toggleLights();
        GameService.Instance.GetInstructionView().HideInstruction();
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.SwitchSound);
    }

    private void toggleLights()
    {
        bool lights = false;

        switch (currentState)
        {
            case SwitchState.On:
                currentState = SwitchState.Off;
                lights = false;
                break;
            case SwitchState.Off:
                currentState = SwitchState.On;
                lights = true;
                break;
            case SwitchState.Unresponsive:
                break;
        }
        foreach (Light lightSource in lightsources)
        {
            lightSource.enabled = lights;
        }
    }
}
