using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

namespace FlashTag
{
	[Library]
	public partial class FlashTagCrosshair : Panel
	{
		public FlashTagCrosshair()
		{
			StyleSheet.Load( "UI/Crosshair/FlashTagCrosshairStyle.scss" );
			AddClass( "base" );
			Add.Panel( "crosshair" );
		}
	}
}
