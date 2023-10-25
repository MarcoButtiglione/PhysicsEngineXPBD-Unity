using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalWorld : MonoBehaviour
{
    public float[] gravity;
    public int numSubsteps = 10;
    public bool paused = false;

    public SoftBody[] softBodies;
    private int _totFixed;

    private bool _isPressed;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _isPressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (_isPressed)
        {
            _isPressed = false;
            softBodies[0].translate(0f,1f,0f);
        }
        Simulate();
        print(_totFixed++);
    }

    private void Simulate()
    {
        if (paused)
            return;

        var sdt = Time.fixedDeltaTime / numSubsteps;

        for (var step = 0; step < numSubsteps; step++) {

            for (var i = 0; i < softBodies.Length; i++) 
                softBodies[i].PreSolve(sdt, gravity);
					
            for (var i = 0; i < softBodies.Length; i++) 
                softBodies[i].Solve(sdt);

            for (var i = 0; i < softBodies.Length; i++) 
                softBodies[i].PostSolve(sdt);
            
        }
    }
}