using Assets.Scripts.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Spikes
{
    public class Spike : MonoBehaviour
    {
        /// <summary>
        /// Distance between consecutive circles
        /// </summary>
        public float radiusStep = 1f;

        /// <summary>
        /// A measure for the height of each spike
        /// </summary>
        public float averageHeight = 1;
        /// <summary>
        /// A measure for the maximal difference between 
        /// the actual height of a spike and the average height
        /// </summary>
        public float heightDeviation = 0;

        /// <summary>
        /// A mesure for the position of the top of a spike
        /// relative to the centroid of the base
        /// </summary>
        public float maxTopDisplacement = 0;

        /// <summary>
        /// The number of rings. The inner-most ring has one spike
        /// and the number of spikes increases by one in each outer
        /// ring.
        /// </summary>
        public int count = 1;

        /// <summary>
        /// A measure which determines the average shape of triangles
        /// which form the bases of each pyramid
        /// </summary>
        [Range(0, Mathf.PI)]
        public float range;

        /// <summary>
        /// The distance between rings which share points in a triangle
        /// </summary>
        public int connectedRingDistance = 5;

        private PolarVector origin;
        private PolarSystem system;

        // Mesh components
        private List<Vector3> vertices;
        private List<int> triangles;

        private float shapeRadius;

        public void Start()
        {
            // Setup coordinate system relative to the position of this component
            origin = new PolarVector(transform.position);
            system = new PolarSystem(origin);

            // Initialise mesh components
            vertices = new List<Vector3>();
            triangles = new List<int>();

            // Create empty mesh
            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            // Setup collider
            MeshCollider collider = GetComponent<MeshCollider>();
            collider.sharedMesh = mesh;

            // TODO : Generate mesh
            GenerateMesh();
            
            // Assign mesh components to the created mesh
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
        }

        private void GenerateMesh()
        {
            // Hold the relative origin
            PolarVector origin;

            // Iterate over each ring starting from the inner-most
            for (int i = 1; i <= count; i += i)
            {
                // Generate the specified number of triangles in each ring
                // (starting with one and increasing by one on each ring)
                for (int j = 0; j < i; j++)
                {
                    // Set the relative origin to a random point on the current ring
                    origin = system.GetPointOnCircle(radiusStep * (i - 1));
                    GeneratePyramid(origin, radiusStep * (i + connectedRingDistance));
                }
            }
        }

        /// <summary>
        /// Generate the part of the mesh which represents a single pyramid
        /// by a given inner base vertex and distance from it.
        /// </summary>
        /// <param name="origin">The inner vertex of the base triangle</param>
        /// <param name="radius">The distance between the inner vertex and the other two</param>
        private void GeneratePyramid(PolarVector origin, float radius)
        {
            // Get two more points on the next circle to form a triangle with the origin
            PolarVector pointB = system.GetPointOnCircle(radius, origin.Azimuth, origin.Azimuth + range);
            PolarVector pointC = system.GetPointOnCircle(radius, origin.Azimuth - range, origin.Azimuth);

            // Find top for pyramid by calculating the centroid of the base
            // and applying random displacement
            Vector3 top = (origin.Cartesian + pointB.Cartesian + pointC.Cartesian) / 3;
            Vector3 topDisplacement = new Vector3(
                UnityEngine.Random.Range(-maxTopDisplacement, maxTopDisplacement),
                averageHeight + UnityEngine.Random.Range(-heightDeviation, heightDeviation),
                UnityEngine.Random.Range(-maxTopDisplacement, maxTopDisplacement));

            // We no longer need the polar representation of these points so we can just pass the cartesian
            SetPyramid(origin.Cartesian, pointB.Cartesian, pointC.Cartesian, top + topDisplacement);
        }

        // http://docs.unity3d.com/Manual/AnatomyofaMesh.html
        private void SetPyramid(Vector3 baseA, Vector3 baseB, Vector3 baseC, Vector3 top)
        {
            SetTriangle(baseA, baseB, baseC);
            SetTriangle(baseA, baseC, top);
            SetTriangle(baseA, top, baseB);
            SetTriangle(baseB, top, baseC);
        }

        private void SetTriangle(Vector3 a, Vector3 b, Vector3 c)
        {
            SetVertex(c);
            SetVertex(b);
            SetVertex(a);
        }

        private void SetVertex(Vector3 vertex)
        {
            triangles.Add(vertices.Count);
            vertices.Add(vertex);
        }
    }
}
