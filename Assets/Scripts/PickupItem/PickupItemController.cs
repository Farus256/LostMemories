using UnityEngine;

public abstract class PickupItemController : InteractionController
{
    public KeyCode dropKey = KeyCode.G;
    protected bool isHeld = false;
    protected Rigidbody rb;
    protected Collider col;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
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
