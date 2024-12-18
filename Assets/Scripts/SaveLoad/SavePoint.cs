using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SavePoint : MonoBehaviour, IInterctable
{
    public SpriteRenderer sprite;
    public GameObject lightObj;
    public Sprite savedSprite;
    public Sprite unsavedSprite;
    [Header("Event broadcaster")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;
    [Header("Status")]
    public bool isSaved;


    private void Awake()
    {
        sprite.sprite = isSaved ? savedSprite : unsavedSprite;

    }

    public void TakeAction()
    {
        if (!isSaved)
        {
            isSaved = true;
            sprite.sprite = savedSprite;
            lightObj.SetActive(true);
        }
        saveDataEvent.RaiseEvent();
    }
}
