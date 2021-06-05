using Sandbox;

namespace VehicleTest
{
	[Library( "vehicletest" )]
	public partial class VehicleTestGame : Game
	{
		public VehicleTestGame()
		{
			if ( IsServer )
			{
				new VTestHud();
			}
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new VTestPlayer();
			client.Pawn = player;

			player.Respawn();
		}
	}

}
