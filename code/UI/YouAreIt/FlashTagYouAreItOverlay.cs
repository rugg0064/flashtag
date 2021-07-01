using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

namespace FlashTag
{
	[Library]
	public partial class FlashTagYouAreItOverlay : Panel
	{
		public FlashTagYouAreItOverlay()
		{
			StyleSheet.Load( "UI/YouAreIt/FlashTagYouAreItOverlayStyle.scss" );
			AddClass( "base" );
			Add.Label( "You are it", "label");
		}

		public override void Tick()
		{
			FlashTagPlayer player = (FlashTagPlayer)Local.Pawn;
			SetClass( "enabled", player.isIt );
		}
	}
}
