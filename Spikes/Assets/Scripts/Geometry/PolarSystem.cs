using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Geometry
{
    /// <summary>
    /// 
    /// </summary>
    public class PolarSystem
    {
        // Should only allow a single set of the pole which happens in the constructor by design.
        // This is to ensure that the relative positions of all other vectors do not have to be
        // recomputed if the pole is changed.
        private readonly PolarVector pole;

        public PolarSystem(PolarVector pole)
        {
            this.pole = pole;
        }

        /// <summary>
        /// The centre of the coordinate system. Can only be set through the constructor.
        /// </summary>
        public PolarVector Pole
        {
            get
            {
                return pole;
            }
        }

        /// <summary>
        /// Generate a random position on a circle
        /// </summary>
        /// <param name="radius">Distance from pole</param>
        /// <returns>A polar vector at the specified distance and random angle</returns>
        public PolarVector GetPointOnCircle(float radius)
        {
            return new PolarVector(radius, Random.Range(0, 2 * Mathf.PI), Pole);
        }

        /// <summary>
        /// Generate a random position in a segment of a circle
        /// </summary>
        /// <param name="radius">Distance from pole</param>
        /// <param name="fromAzimuth">Lower angle of segment to include</param>
        /// <param name="toAzimuth">Higher angle of segment to include</param>
        /// <returns>A polar vector which lies between the two given angles at the specified distance</returns>
        public PolarVector GetPointOnCircle(float radius, float fromAzimuth, float toAzimuth)
        {
            return new PolarVector(radius, Random.Range(fromAzimuth, toAzimuth), Pole);
        }

        /// <summary>
        /// Generate a random position on a circle inside a segment range relative to another vector.
        /// The segment is determined by adding the given range on each side of the azimuth of the given vector.
        /// </summary>
        /// <param name="radius">Distance from pole</param>
        /// <param name="azimuthRange">Magnitude of segment range in each direction (in radians)</param>
        /// <param name="relativeTo">A vector whose azimuth is used as the centre of the segment</param>
        /// <returns></returns>
        public PolarVector GetPointOnCircle(float radius, float azimuthRange, PolarVector relativeTo)
        {
            return new PolarVector(radius, Random.Range(relativeTo.Azimuth - azimuthRange, relativeTo.Azimuth + azimuthRange), Pole);
        }
    }
}
