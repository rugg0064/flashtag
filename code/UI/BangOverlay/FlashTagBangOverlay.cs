using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

namespace FlashTag
{
	[Library]
	public partial class FlashTagBangOverlay : Panel
	{
		public FlashTagBangOverlay()
		{
			StyleSheet.Load( "UI/BangOverlay/FlashTagBangOverlayStyle.scss" );
			AddClass( "overlay" );
		}

		public override void Tick()
		{
			base.Tick();
			FlashTagPlayer player = (FlashTagPlayer)Local.Pawn;
			float lastFlashTime = player.lastFlashedTime;
			//float flashDuration = 3.5f;
			float timeBeenFlashed = Time.Now - lastFlashTime;
			//float percent = MathF.Max( 0, MathF.Min( timeBeenFlashed / flashDuration, 1 ) );

			float percent = 1 - MathF.Max( 0, MathF.Min( ((timeBeenFlashed - 2) / 3), 1 ));

			if ( player.LifeState == LifeState.Dead )
			{
				percent = 0.0f;
			}
			//Log.Info( percent );

			this.Style.Set( "background-color: rgba(255, 255, 255, " + (percent).ToString() + ")");
		}
	}
}
