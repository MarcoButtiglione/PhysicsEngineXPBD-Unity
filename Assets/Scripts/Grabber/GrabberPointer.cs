using System.Collections.Generic;
using UnityEngine;

//General class to grab objects with mouse and throw them around
namespace Grabber
{
    public class GrabberPointer: IGrabber
    {
        //Data needed 
        private readonly Camera _mainCamera;
        //The mesh we grab
        private IGrabbable _grabbedBody;
        //Mesh grabbing data
        //When we have grabbed a mesh by using ray-triangle itersection we identify the closest vertex. The distance from camera to this vertex is constant so we can move it around without doing another ray-triangle itersection  
        private float _distanceToGrabPos;
        //To give the mesh a velocity when we release it
        private Vector3 _lastGrabPos;
        
        public GrabberPointer(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }
        
        public void StartGrab(List<IGrabbable> bodies)
        {
            if (_grabbedBody != null)
            {
                return;
            }

            //A ray from the mouse into the scene
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            float maxDist = float.MaxValue;

            IGrabbable closestBody = null;

            CustomHit closestHit = default;

            foreach (IGrabbable body in bodies)
            {
                body.IsRayHittingBody(ray, out CustomHit hit);

                if (hit != null)
                {
                    //Debug.Log("Ray hit");

                    //Debug.Log(hit.distance);

                    if (hit.distance < maxDist)
                    {
                        closestBody = body;

                        maxDist = hit.distance;

                        closestHit = hit;
                    }
                }
                else
                {
                    //Debug.Log("Ray missed");
                }
            }

            if (closestBody != null)
            {
                _grabbedBody = closestBody;
        
                //StartGrab is finding the closest vertex and setting it to the position where the ray hit the triangle
                closestBody.StartGrab(closestHit.location);

                _lastGrabPos = closestHit.location;

                //distanceToGrabPos = (ray.origin - hit.location).magnitude;
                _distanceToGrabPos = closestHit.distance;
            }
        }
    
        public void MoveGrab(Vector3 mousePosition)
        {
            if (_grabbedBody == null)
            {
                return;
            }

            //A ray from the mouse into the scene
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            Vector3 vertexPos = ray.origin + ray.direction * _distanceToGrabPos;

            //Cache the old pos before we assign it
            _lastGrabPos = _grabbedBody.GetGrabbedPos();

            //Moved the vertex to the new pos
            _grabbedBody.MoveGrabbed(vertexPos);
        }
    
        public void EndGrab(Vector3 mousePosition)
        {
            if (_grabbedBody == null)
            {
                return;
            }

            //Add a velocity to the ball

            //A ray from the mouse into the scene
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            Vector3 grabPos = ray.origin + ray.direction * _distanceToGrabPos;

            float vel = (grabPos - _lastGrabPos).magnitude / Time.deltaTime;
        
            Vector3 dir = (grabPos - _lastGrabPos).normalized;

            _grabbedBody.EndGrab(grabPos, dir * vel);

            _grabbedBody = null;
        }
    }
}