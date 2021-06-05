using Sandbox;

namespace VehicleTest
{
	[Library( "box_spawner" )]
	class VTestBoxSpawner : VTestCarriable
	{
		TimeSince timeSinceShoot;

		public override void Simulate( Client cl )
		{
			if ( Owner == null )
				return;

			if ( Host.IsServer )
			{
				if ( Owner.Input.Pressed( InputButton.Attack1 ) )
				{
					SpawnBoxVehicle();
				}
			}
		}

		void SpawnBoxVehicle()
		{
			var tr = Trace.Ray( Owner.EyePos, Owner.EyePos + Owner.EyeRot.Forward * 500 )
				.UseHitboxes()
				.Ignore( Owner )
				.Size( 2 )
				.Run();

			var ent = new VTestBox();
			ent.Position = tr.EndPos;
			ent.Rotation = Rotation.From( new Angles( 0, Owner.EyeRot.Angles().yaw, 0 ) );
		}
	}
}
