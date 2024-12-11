using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInterctable
{
    public Sprite openChest;
    public Sprite closeChest;
    public bool isDone;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        sprite.sprite = isDone ? openChest : closeChest;
    }
    public void TakeAction()
    {
        isDone = true;
        sprite.sprite = openChest;

        // remove collider or remove tag
        Destroy(gameObject.GetComponent<Collider2D>());
    }
}
