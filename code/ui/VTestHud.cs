using Sandbox.UI;

namespace VehicleTest
{
	public partial class VTestHud : Sandbox.HudEntity<RootPanel>
	{
		public VTestHud()
		{
			if ( !IsClient )
				return;

			RootPanel.StyleSheet.Load( "/ui/VTestHud.scss" );

			RootPanel.AddChild<NameTags>();
			RootPanel.AddChild<CrosshairCanvas>();
			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<VoiceList>();
			RootPanel.AddChild<KillFeed>();
			RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
			RootPanel.AddChild<VTestHealth>();
			RootPanel.AddChild<VTestInventoryBar>();
			RootPanel.AddChild<VTestTitleText>();
		}
	}

}
