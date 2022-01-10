using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrail : MonoBehaviour {

    private TrailRenderer trailRenderer;

    public bool isMouseDragged;

    // Start is called before the first frame update
    void Start() {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            trailRenderer.enabled = true;
            isMouseDragged = true;

            Plane objPlane = new Plane(
                Camera.main.transform.forward * -1,
                transform.position
            );

            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
                transform.position = mRay.GetPoint(rayDistance);
        } else if (Input.GetKeyUp(KeyCode.Mouse0)) {
            trailRenderer.enabled = false;
            isMouseDragged = false;
        }
    }
}
