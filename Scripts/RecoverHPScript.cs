using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverHPScript : MonoBehaviour
{
    public float recoverAmount = 1f; 
    private bool pickedUp = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickedUp) return;

        LunaBehaviourNew luna = collision.GetComponentInParent<LunaBehaviourNew>();
        if (luna == null)
        {
            Debug.Log($"Recover pickup collided with '{collision.gameObject.name}' but no LunaBehaviourNew found.");
            return;
        }

        int amount = Mathf.RoundToInt(recoverAmount);
        Debug.Log($"Recover pickup: target='{luna.gameObject.name}', current={luna.currenthealth}, max={luna.maxhealth}, add={amount}");

        luna.ChangeHealth(amount);

        pickedUp = true;
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        var sprite = GetComponent<SpriteRenderer>();
        if (sprite != null) sprite.enabled = false;

        Destroy(gameObject, 0.05f);
    }
}
