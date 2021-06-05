using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace VehicleTest {
	public class VTestTitleText : Panel
	{
		public Label Label;

		public VTestTitleText()
		{
			Label = Add.Label( "Vehicle Test", "value" );
		}
	}
}
