using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace VehicleTest {
    public class VTestHealth : Panel
    {
        public Label Label;

        public VTestHealth()
        {
            Label = Add.Label( "100", "value" );
        }

        public override void Tick()
        {
            var player = Local.Pawn;
            if ( player == null ) return;

            Label.Text = $"{player.Health.CeilToInt()}";
        }
    }
}
