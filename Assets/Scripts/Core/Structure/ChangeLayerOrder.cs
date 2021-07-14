using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defined for every moving objects entering the trigger range will decrease pic render priority for creating 3d sense
public class ChangeLayerOrder : MonoBehaviour
{
    // When minion, monster, hero enter the range, change layer render order
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hero" || collision.gameObject.tag == "Minion" || collision.gameObject.tag == "Monster")
        {
            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hero" || collision.gameObject.tag == "Minion" || collision.gameObject.tag == "Monster")
        {
            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 3;
        }
    }
}
