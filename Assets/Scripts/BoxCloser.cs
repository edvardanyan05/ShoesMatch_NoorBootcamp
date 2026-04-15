using UnityEngine;

public class BoxCloser : MonoBehaviour
{

    public GameObject openBox;
    public GameObject closedBox;

    void Start()
    {
        openBox.SetActive(true);
        closedBox.SetActive(false);
    }
    public void Close()
    {
        openBox.SetActive(false);
        closedBox.SetActive(true);
    }
}
