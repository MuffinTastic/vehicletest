using Sandbox;

namespace VehicleTest
{
	partial class VTestPlayer
	{
		protected bool IsUseDisabled()
		{
			return ActiveChild is IUse use && use.IsUsable( this );
		}

		protected override Entity FindUsable()
		{
			if ( IsUseDisabled() )
				return null;

			var tr = Trace.Ray( EyePos, EyePos + EyeRot.Forward * 85 )
				.Radius( 2 )
				.HitLayer( CollisionLayer.Debris )
				.Ignore( this )
				.Run();

			if ( tr.Entity == null ) return null;
			if ( tr.Entity is not IUse use ) return null;
			if ( !use.IsUsable( this ) ) return null;

			return tr.Entity;
		}

		/// <summary>
		/// This should be called somewhere in your player's tick to allow them to use entities
		/// </summary>
		protected override void TickPlayerUse()
		{
			// This is serverside only
			if ( !Host.IsServer ) return;

			// Turn prediction off
			//using ( Prediction.Off() )
			{
				if ( Input.Pressed( InputButton.Use ) )
				{
					Using = FindUsable();

					if ( Using == null )
					{
						UseFail();
						return;
					}
				}

				if ( !Input.Down( InputButton.Use ) )
				{
					StopUsing();
					return;
				}

				if ( !Using.IsValid() )
					return;

				// If we move too far away or something we should probably ClearUse()?

				//
				// If use returns true then we can keep using it
				//
				if ( Using is IUse use && use.OnUse( this ) )
					return;

				StopUsing();
			}
		}

		protected override void UseFail()
		{
			if ( IsUseDisabled() )
				return;

			base.UseFail();
		}
	}
}
