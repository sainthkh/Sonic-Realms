﻿using Hedgehog.Core.Actors;
using Hedgehog.Core.Triggers;
using Hedgehog.Core.Utils;
using UnityEngine;

namespace Hedgehog.Level.Platforms
{
    /// <summary>
    /// Moves the player forward or backward on the platform.
    /// </summary>
    public class ConveyorBelt : ReactivePlatform
    {
        /// <summary>
        /// The player is moved by this amount on the surface, in units per second. Positive means
        /// forward, negative means backward.
        /// </summary>
        [SerializeField] public float Velocity;

        private float _lastSurfaceAngle;

        public void Reset()
        {
            Velocity = 2.5f;
        }

        // Translate the controller by the amount defined in Velocity and the direction defined by its
        // surface angle.
        public override void OnSurfaceStay(HedgehogController controller, TerrainCastHit hit, SurfacePriority priority)
        {
            controller.Translate(DMath.AngleToVector2(controller.SurfaceAngle*Mathf.Deg2Rad)*Velocity*
                                 Time.fixedDeltaTime);

            _lastSurfaceAngle = controller.SurfaceAngle;
        }

        // Transfer momentum to the controller when it leaves the conveyor belt.
        public override void OnSurfaceExit(HedgehogController controller, TerrainCastHit hit, SurfacePriority priority)
        {
            if (controller.Grounded)
                controller.GroundVelocity += Velocity;
            else
                controller.Velocity += DMath.AngleToVector2(_lastSurfaceAngle*Mathf.Deg2Rad)*Velocity;
        }
    }
}