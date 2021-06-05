using System;
using Sandbox;

namespace VehicleTest
{
	public class VTestVehicle : Prop, IUse, IPhysicsUpdate
	{
		public virtual string ModelPath => null;

		protected struct VehicleInputState
		{
			public Vector3 desiredMovement;
			public float desiredThrottle;
			public Rotation cameraRotation;
			public Vector3 mouseDelta;

			public void Reset()
			{
				desiredMovement = Vector3.Zero;
				desiredThrottle = 0.0f;
				cameraRotation = Rotation.Identity;
				mouseDelta = Vector2.Zero;
			}
		}

		protected VehicleInputState InputState;

		public override void Spawn()
		{
			Camera = new VTestVehicleCamera();
			base.Spawn();

			SetModel( ModelPath );
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
		}

		public bool OnUse( Entity player )
		{
			EnterVehicle( player );

			return false;
		}

		public bool IsUsable( Entity player ) => true;

		private void EnterVehicle( Entity player )
		{
			if ( player == null ) return;

			var client = player.GetClientOwner();

			ActiveChild = player; // I hope this is right
			client.Pawn = this;
		}

		private void ExitVehicle()
		{
			var player = ActiveChild;
			if ( player == null ) return;

			var client = GetClientOwner();

			client.Pawn = player;
			ActiveChild = null;
		}
		public virtual void OnPostPhysicsStep( float dt )
		{
			if ( !PhysicsBody.IsValid() )
				return;
		}

		public override void Simulate( Client cl )
		{
			if ( cl == null ) return;
			if ( !IsServer ) return;

			using ( Prediction.Off() )
			{
				var input = cl.Input;

				if ( input.Pressed( InputButton.Use ) )
				{
					ExitVehicle();
					return;
				}

				InputState.Reset();

				InputState.desiredMovement = new Vector3(
					(input.Down( InputButton.Forward ) ? 1.0f : 0.0f) + (input.Down( InputButton.Back ) ? -1.0f : 0.0f),
					(input.Down( InputButton.Left ) ? 1.0f : 0.0f) + (input.Down( InputButton.Right) ? -1.0f : 0.0f),
					(input.Down( InputButton.Jump ) ? 1.0f : 0.0f) + (input.Down( InputButton.Duck ) ? -1.0f : 0.0f)
				);

				InputState.desiredThrottle = input.Down( InputButton.Run ) ? 2.0f : (input.Down( InputButton.Walk ) ? 0.5f : 1.0f);

				InputState.cameraRotation = Input.Rotation;
				InputState.mouseDelta = input.MouseDelta;
			}
		}

		public override void TakeDamage( DamageInfo info )
		{
			// Log.Info( $"Hitbox {GetBoneName( GetHitboxBone( info.HitboxIndex ) )}" );

			base.TakeDamage( info );
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			ExitVehicle();
		}
	}
}
