using UnityEngine;

//Should be class so it can be null
namespace XPBD_Engine.Scripts.Utilities.Data_structures
{
    public class PointerHit
    {
        //The distance along the ray to where the ray hit the object
        public float distance;

        //Point of intersection
        public Vector3 location;
        //Normal of the surface where the ray hit
        public Vector3 normal; 

        //What is index? Currently assumed to be the index of the first vertex of the triangle
        public int index;

        public PointerHit(float distance, Vector3 location, Vector3 normal, int index = -1)
        {
            this.distance = distance;
            this.location = location;
            this.normal = normal;
            this.index = index;
        }
    }
}
