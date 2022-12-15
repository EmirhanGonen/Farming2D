using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] private GameObject @object;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Player player)) return;
        DoScale(@object, 1, .5f);
    }

    private void DoScale(GameObject @object, float scale, float time) => @object.transform.DOScale(scale, time).SetEase(Ease.Linear);


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Player player)) return;
        DoScale(@object, 0, .5f);
    }
}