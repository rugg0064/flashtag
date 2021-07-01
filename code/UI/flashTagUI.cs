using Sandbox;
using Sandbox.UI;

namespace FlashTag
{
	[Library]
	public partial class flashTagUI : HudEntity<RootPanel>
	{
		public flashTagUI()
		{
			if ( !IsClient )
				return;
			RootPanel.AddChild<FlashTagBangOverlay>();

			RootPanel.AddChild<FlashTagCrosshair>();

			RootPanel.AddChild<FlashTagYouAreItOverlay>();

			RootPanel.AddChild<FlashTagScoreboard>();
		}
	}
}
