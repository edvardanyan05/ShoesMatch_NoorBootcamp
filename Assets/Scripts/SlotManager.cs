using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ColorSlot
{
    public ShoeColor color;
    public Transform slot;
    public BoxCloser boxCloser;
}

public class SlotManager : MonoBehaviour
{
    public Transform[] slots;
    public ColorSlot[] finalSlots;

    public GameObject GameOverPanel;
    public GameObject LevelCompletePanel;

    private List<Shoe> shoes = new List<Shoe>();
    private List<Transform> occupiedSlots = new List<Transform>();
    private int closedBoxes = 0;
    public Transform GetEmptySlot()
    {
        foreach (var slot in slots)
        {
            if (!occupiedSlots.Contains(slot))
            {
                occupiedSlots.Add(slot);
                return slot;
            }
        }
        return null;
    }

    public void OnShoeArrived(Shoe shoe)
    {
        if (!shoes.Contains(shoe))
            shoes.Add(shoe);

        CheckPair();
        if (shoes.Count >= slots.Length)
            CheckGameOver();
    }

    void CheckGameOver()
    {
        for (int i = 0; i < shoes.Count; i++)
            for (int j = i + 1; j < shoes.Count; j++)
                if (shoes[i].shoeColor == shoes[j].shoeColor)
                    return;

        GameOverPanel.SetActive(true);
        Invoke("RestartGame", 2f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void CheckPair()
    {
        for (int i = 0; i < shoes.Count; i++)
        {
            for (int j = i + 1; j < shoes.Count; j++)
            {
                if (shoes[i].shoeColor == shoes[j].shoeColor)
                {
                    SendPairToBox(shoes[i], shoes[j]);
                    return;
                }
            }
        }
    }

    void SendPairToBox(Shoe a, Shoe b)
    {
        if (a.currentSlot != null) occupiedSlots.Remove(a.currentSlot);
        if (b.currentSlot != null) occupiedSlots.Remove(b.currentSlot);

        shoes.Remove(a);
        shoes.Remove(b);

        Transform target = GetFinalSlot(a.shoeColor);
        if (target == null) return;

        a.currentSlot = null;
        b.currentSlot = null;

        Vector3 basePos = target.position;
        a.MoveTo(basePos + Vector3.left * 0.3f);
        b.MoveTo(basePos + Vector3.right * 0.3f);

        colorQueue.Enqueue(a.shoeColor);
        Invoke("CloseBox", 3f);
    }

    private Queue<ShoeColor> colorQueue = new Queue<ShoeColor>();

    public int pairCount;
    void CloseBox()
    {
        if (colorQueue.Count == 0) return;
        ShoeColor lastColor = colorQueue.Dequeue();

        foreach (var cs in finalSlots)
        {
            if (cs.color == lastColor)
            {
                if (cs.boxCloser != null)
                {
                    cs.boxCloser.Close();
                    closedBoxes++;
                }
                break;
            }
        }
        
        if (closedBoxes == pairCount)
        {
            LevelComplete();
        }
    }

    void LevelComplete()
    {
        LevelCompletePanel.SetActive(true);
        Invoke("LoadNextLevel", 2f);
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }   
    Transform GetFinalSlot(ShoeColor color)
    {
        foreach (var cs in finalSlots)
            if (cs.color == color) return cs.slot;
        return null;
    }
}