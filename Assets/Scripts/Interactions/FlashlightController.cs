using System;
using UnityEngine;
using UnityEngine.Rendering;

public class FlashlightController : PickupItemController
{
    public KeyCode toggleLightKey = KeyCode.F;
    public Light flashlightLight;
    public Vector3 heldPosition;
    public Vector3 heldRotation;

    private bool isOn = false;
    private MeshRenderer meshRenderer;

    protected override void Start()
    {
        base.Start();
        if (flashlightLight != null)
            flashlightLight.enabled = false;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        if (isHeld)
        {
            if (Input.GetKeyDown(toggleLightKey))
            {
                isOn = !isOn;
                flashlightLight.enabled = isOn;
            }
        }
    }
    
    protected override void PickUp()
    {
        isHeld = true;
        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(m_PlayerCamera.transform);
        transform.localPosition = heldPosition;
        transform.localEulerAngles = heldRotation;
        meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
    }
    
    protected override void Drop()
    {
        isHeld = false;
        rb.isKinematic = false;
        col.enabled = true;
        transform.SetParent(null);
        meshRenderer.shadowCastingMode = ShadowCastingMode.On;
        rb.AddForce(m_PlayerCamera.transform.forward * throwForce, ForceMode.VelocityChange);
    }

    public void ToggleFlashLightOnce()
    {
        isOn = !isOn;
        flashlightLight.enabled = isOn;
    }
}
