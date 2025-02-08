using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float grappleLenght;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer rope;

    Vector2 grapplePoint;
    private SpringJoint2D joint;
   

    void Start()
    {
        joint = gameObject.AddComponent<SpringJoint2D>();
        joint.enabled = false;
        rope.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit2D hit = Physics2D.Raycast(
                origin: Camera.main.ScreenToWorldPoint(Input.mousePosition),
                direction: Vector2.zero,
                distance: Mathf.Infinity,
                layerMask: grappleLayer
                );

            if (hit.collider != null) { 
                grapplePoint = hit.point;
                joint.enabled = true;
                joint.distance = grappleLenght;
                joint.dampingRatio = grappleLayer/10;
                joint.frequency = 2f;
                rope.SetPosition(0, grapplePoint);
                rope.SetPosition(1, transform.position);
                rope.enabled = true;
            }
        }

        if (Input.GetMouseButtonUp(1)) {
            joint.enabled = false;
            rope.enabled = false;
        }

        if (rope.enabled == true) {
            rope.SetPosition(1, transform.position);
        }
    }
}
