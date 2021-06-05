using Sandbox;

namespace VehicleTest
{
	public partial class VTestCarriable : BaseCarriable, IUse
	{
		public override void CreateViewModel()
		{
			Host.AssertClient();

			if ( string.IsNullOrEmpty( ViewModelPath ) )
				return;

			ViewModelEntity = new VTestViewModel
			{
				Position = Position,
				Owner = Owner,
				EnableViewmodelRendering = true
			};

			ViewModelEntity.SetModel( ViewModelPath );
		}

		public bool OnUse( Entity user )
		{
			return false;
		}

		public virtual bool IsUsable( Entity user )
		{
			return Owner == null;
		}
	}
}
