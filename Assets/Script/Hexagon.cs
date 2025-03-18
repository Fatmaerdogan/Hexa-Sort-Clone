using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using DG.Tweening; 

public class Hexagon : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private new Renderer renderer;
    [SerializeField] private new Collider collider;
    public HexagonStack HexagonStack { get; private set; }


    public Material MaterialSet
    {
        get => renderer.material;
        set => renderer.material = value;
    }

    public void DisableCollider() => collider.enabled = false;
    public void Configure(HexagonStack hexStack)
    {
        HexagonStack = hexStack;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void MoveToLocal(Vector3 targetLocalPos)
    {
        LeanTween.cancel(gameObject);

        float delay = transform.GetSiblingIndex() * .01f;

        LeanTween.moveLocal(gameObject, targetLocalPos, .2f)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(delay);

        Vector3 direction = (targetLocalPos - transform.localPosition).With(y: 0).normalized;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        LeanTween.rotateAround(gameObject, rotationAxis, 180, .2f)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(delay);
    }


    public void Vanish(float delay)
    {
        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, Vector3.zero, .2f)
            .setEase(LeanTweenType.easeInBack)
            .setDelay(delay)
            .setOnComplete(() => Destroy(gameObject));
    }

}
