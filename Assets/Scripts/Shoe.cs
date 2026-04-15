using UnityEngine;

public class Shoe : MonoBehaviour
{
    public SlotManager container;
    public float speed = 5f;
    public ShoeColor shoeColor;

    public Transform currentSlot;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool arrivedNotified = false;

    void Update()
    {
        HandleMovement();
        HandleInput();
    }

    void HandleMovement()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            isMoving = false;

            if (!arrivedNotified && currentSlot != null)
            {
                arrivedNotified = true;
                container.OnShoeArrived(this);
            }
        }
    }

    void HandleInput()
    {
        if (isMoving || currentSlot != null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            {
                MoveToContainer();
            }
        }
    }

    void MoveToContainer()
    {
        Transform slot = container.GetEmptySlot();
        if (slot == null) return;

        currentSlot = slot;
        arrivedNotified = false;
        MoveTo(slot.position);
    }

    public void MoveTo(Vector3 pos)
    {
        targetPosition = pos;
        isMoving = true;
        if (arrivedNotified)
            currentSlot = null;
    }
}

public enum ShoeColor { Red, Blue, Green, Yellow, Purple, Orange }