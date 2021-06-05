using Sandbox;
using System;

namespace VehicleTest
{
	class VTestBox : VTestVehicle
	{
		public override string ModelPath => "models/citizen_props/crate01.vmdl";

		public virtual float movementAcceleration => 3000;
		public virtual float yawSpeed => 30;
		public virtual float uprightSpeed => 5000;
		public virtual float uprightDot => 0.5f;
		public virtual float leanWeight => 0.5f;
		public virtual float leanMaxVelocity => 1000;

		public override void Spawn()
		{
			base.Spawn();

			Camera = new VTestVehicleCamera( 50.0f );
		}

		public override void OnPostPhysicsStep( float dt )
		{
			if ( !PhysicsBody.IsValid() )
			{
				return;
			}

			var body = PhysicsBody;
			var transform = Transform;

			body.LinearDrag = 1.0f;
			body.AngularDrag = 1.0f;
			body.LinearDamping = 4.0f;
			body.AngularDamping = 4.0f;

			var yawRot = Rotation.From( new Angles( 0, Rotation.Angles().yaw, 0 ) );
			var worldMovement = yawRot * InputState.desiredMovement.Normal;
			var velocityDirection = body.Velocity.WithZ( 0 );
			var velocityMagnitude = velocityDirection.Length;
			velocityDirection = velocityDirection.Normal;

			var velocityScale = (velocityMagnitude / leanMaxVelocity).Clamp( 0, 1 );
			var leanDirection = worldMovement.LengthSquared == 0.0f
				? -velocityScale * velocityDirection
				: worldMovement;

			var targetUp = (Vector3.Up + leanDirection * leanWeight * velocityScale).Normal;
			var currentUp = transform.NormalToWorld( Vector3.Up );
			var alignment = MathF.Max( Vector3.Dot( targetUp, currentUp ), 0 );

			bool hasCollision = false;
			bool isGrounded = false;

			if ( !hasCollision || isGrounded )
			{
				var hoverForce = isGrounded && InputState.desiredThrottle <= 0 ? Vector3.Zero : -1 * transform.NormalToWorld( Vector3.Up ) * -800.0f;
				var movementForce = isGrounded ? Vector3.Zero : worldMovement * InputState.desiredThrottle * movementAcceleration;
				var totalForce = hoverForce + movementForce;
				body.ApplyForce( (totalForce * alignment) * body.Mass );
			}

			if ( !hasCollision && !isGrounded )
			{
				var spinDiff = 0.0f;

				if ( ActiveChild != null )
				{
					spinDiff = InputState.cameraRotation.Yaw() - yawRot.Yaw();
					if ( MathF.Abs( spinDiff ) >= 180.0f ) spinDiff -= MathF.Sign( spinDiff ) * 360.0f;
				}

				var spinTorque = Transform.NormalToWorld( new Vector3( 0, 0, spinDiff * yawSpeed ) );
				var uprightTorque = Vector3.Cross( currentUp, targetUp ) * uprightSpeed;
				var uprightAlignment = alignment < uprightDot ? 0 : alignment;
				var totalTorque = spinTorque * alignment + uprightTorque * uprightAlignment;
				body.ApplyTorque( (totalTorque * alignment) * body.Mass );
			}
		}
	}
}
