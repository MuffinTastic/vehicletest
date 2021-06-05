using Sandbox;

namespace VehicleTest
{
	public partial class VTestWeapon : BaseWeapon, IUse
	{
		public PickupTrigger PickupTrigger { get; protected set; }

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );

			PickupTrigger = new PickupTrigger
			{
				Parent = this,
				Position = Position,
				EnableTouch = true
			};
		}

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
			if ( Owner != null )
				return false;

			if ( !user.IsValid() )
				return false;

			user.StartTouch( this );

			return false;
		}

		public virtual bool IsUsable( Entity user )
		{
			return Owner == null;
		}

		public void Remove()
		{
			PhysicsGroup?.Wake();
			Delete();
		}
	}
}
