using UnityEngine;

public abstract class PickupItemController : InteractionController
{
    public KeyCode dropKey = KeyCode.G;
    public float throwForce = 5f;
    protected bool isHeld = false;
    protected Rigidbody rb;
    protected Collider col;


    private Material originalMaterial;
    public Material highlightMaterial;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (GetComponent<Renderer>())
        {
            originalMaterial = GetComponent<Renderer>().material;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (isHeld)
        {
            if (Input.GetKeyDown(dropKey))
            {
                Drop();
            }
        }
        else
        {
            UpdateHighlight();
        }
    }

    private void UpdateHighlight()
    {
        if (IsPlayerNear() && IsLookingAtObject())
        {
            HighlightObject(true);
        }
        else
        {
            HighlightObject(false);
        }
    }

    private void HighlightObject(bool highlight)
    {
        /*if (highlight && GetComponent<Renderer>())
        {
            GetComponent<Renderer>().material = highlightMaterial;
        }
        else if (GetComponent<Renderer>())
        {
            GetComponent<Renderer>().material = originalMaterial;
        }*/
    }

    protected override void Interaction()
    {
        if (!isHeld)
        {
            PickUp();
        }
    }

    protected abstract void PickUp();
    protected abstract void Drop();
}
