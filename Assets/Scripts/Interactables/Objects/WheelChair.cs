using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelChair : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;

    public bool Interact(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }
}
