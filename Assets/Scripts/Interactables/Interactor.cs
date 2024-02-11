using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{

    public bool wireframeEnabled;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask interactionMask;

    private readonly Collider[] colliders = new Collider [3];
    [SerializeField] private int numFound;
    void Update()
    {
        //Find everything in the sphere that is part of the interactable mask
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactionMask);

    }

    private void OnDrawGizmos()
    {
        if(!wireframeEnabled)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
