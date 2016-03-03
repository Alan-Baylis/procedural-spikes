using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Geometry
{
    /// <summary>
    /// I have decided to keep things simple. This is a straightforward
    /// implementation of polar vector and the only clever thing I am doing is
    /// computing the cartesian representation in advance because I know that I
    /// will need it at some point (probably multiple times). If you want to
    /// optimise it you should probably consider making it a struct and
    /// applying some problem-specific knowledge.
    /// </summary>
    public class PolarVector
    {
        public PolarVector(float radius, float azimuth)
        {
            Radius = radius;
            Azimuth = azimuth;

            Cartesian = ToCartesianVector();
        }

        public PolarVector(float radius, float azimuth, PolarVector pole)
        {
            Radius = radius;
            Azimuth = azimuth;

            Cartesian = ToCartesianVector();
            WorldCartesian = ToCartesianVector(pole);
        }

        public PolarVector(Vector3 cartesianVector)
        {
            Radius = Mathf.Sqrt(cartesianVector.sqrMagnitude);
            Azimuth = Mathf.Atan2(cartesianVector.z, cartesianVector.x);

            Cartesian = cartesianVector;
            WorldCartesian = cartesianVector;
        }

        public float Radius { get; set; }
        public float Azimuth { get; set; }
        public Vector3 Cartesian { get; protected set; }
        public Vector3 WorldCartesian { get; protected set; }

        private Vector3 ToCartesianVector()
        {
            return new Vector3(Radius * Mathf.Cos(Azimuth), 0, Radius * Mathf.Sin(Azimuth));
        }

        /// <summary>
        /// Get a vector if the pole of a polar system is not the same as the Unity world origin
        /// </summary>
        /// <param name="pole">The centre of the polar coordinate system used to work with this vector</param>
        /// <returns>A cartesian vector representation of this vector relative to the Unity world origin</returns>
        private Vector3 ToCartesianVector(PolarVector pole)
        {
            return pole.Cartesian + Cartesian;
        }

    }
}
