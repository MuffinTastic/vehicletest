using Sandbox;

namespace VehicleTest
{
	public class VTestVehicleCamera : Camera
	{
		private float orbitDistance;

		public VTestVehicleCamera()
		{
			orbitDistance = 300.0f;
		}

		public VTestVehicleCamera(float orbit)
		{
			orbitDistance = orbit;
		}

		public override void Update()
		{
			var pawn = Local.Pawn;
			var client = Local.Client;

			if ( pawn == null )
				return;

			Pos = pawn.Position;
			Vector3 targetPos;

			var center = pawn.Position + pawn.Rotation * Vector3.Up * 45;

			Pos = center;
			Rot = client.Input.Rotation;

			float distance = orbitDistance * pawn.Scale;
			targetPos = Pos + client.Input.Rotation.Forward * -distance;

			var tr = Trace.Ray( Pos, targetPos )
				.Ignore( pawn )
				.Radius( 8 )
				.Run();

			Pos = tr.EndPos;

			FieldOfView = 70;

			Viewer = null;
		}
	}
}
