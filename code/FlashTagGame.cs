using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashTag
{
	[Library( "flash_tag" )]
	public partial class FlashTagGame : Sandbox.Game
	{
		[Net]
		public List<FlashTagScoreboardStruct> playerScores { get; private set; }
		public FlashTagGame()
		{
			if ( IsServer )
			{
				playerScores = new List<FlashTagScoreboardStruct>();
				new flashTagUI();
			}

			if ( IsClient )
			{
				
			}
		}

		public void gotFlashed( FlashGrenade grenade, FlashTagPlayer player )
		{
			int playerNetID = player.GetClientOwner().NetworkIdent;
			bool found = false;
			for ( int i = 0; i < playerScores.Count && !found; i++ )
			{
				if ( playerScores[i].netID == playerNetID )
				{
					found = true;
					Log.Info( "dying" );
					playerScores[i] = playerScores[i].giveTag();
				}
			}
			player.getFlashed();
		}

		public void setIt(FlashTagPlayer player)
		{
			player.isIt = true;
			//player.dressEvil();
			dress( player );
			destroyAllGrenades();
		}

		public void resetAllIts()
		{
			FlashTagPlayer[] players = All.OfType<FlashTagPlayer>().ToArray();
			for ( int i = 0; i < players.Length; i++ )
			{
				players[i].isIt = false;
				undress( players[i] );
			}
			destroyAllGrenades();
		}

		public void dress(FlashTagPlayer player)
		{
			string modelString = "addons/citizen/models/citizen_clothes/dress/dress.kneelength.vmdl";
			ModelEntity jacket = new ModelEntity();
			jacket.SetModel( modelString );
			jacket.SetParent( player, true );
			jacket.EnableShadowInFirstPerson = true;
			jacket.EnableHideInFirstPerson = true;
			player.SetBodyGroup( "Chest", 1 );
			player.SetBodyGroup( "Legs", 1 );
			//player.fixTheStuff1();
			string modelString2 = "addons/citizen/models/citizen_clothes/hair/hair_femalebun.black.vmdl";
			ModelEntity hat = new ModelEntity();
			hat.SetModel( modelString2 );
			hat.SetParent( player, true );
			hat.EnableShadowInFirstPerson = true;
			hat.EnableHideInFirstPerson = true;
		}

		public void undress(FlashTagPlayer player)
		{
			player.SetBodyGroup( "Chest", 0 );
			player.SetBodyGroup( "Legs", 0 );
			//player.fixTheStuff2();
			foreach (Entity e in player.Children)
			{
				e.Delete();
			}
		}

		public void destroyAllGrenades()
		{
			FlashGrenade[] grenades = All.OfType<FlashGrenade>().ToArray();
			for(int i = 0; i < grenades.Length; i++ )
			{
				grenades[i].Delete();
			}
		}

		public void pickRandomIts()
		{
			FlashTagPlayer[] players = All.OfType<FlashTagPlayer>().ToArray();
			int playerIndex = Rand.Int( 0, players.Length - 1 );
			Log.Info( players[playerIndex].GetClientOwner().Name );
			players[playerIndex].isIt = true;
			destroyAllGrenades();
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new FlashTagPlayer();
			client.Pawn = player;

			playerScores.Add( new FlashTagScoreboardStruct( client.NetworkIdent ) );

			if( All.OfType<FlashTagPlayer>().Count() == 1)
			{
				player.isIt = true;
				//player.dressEvil();
				dress( player );
			}

			player.Respawn();
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			base.ClientDisconnect( cl, reason );

			bool found = false;
			for ( int i = 0; i < playerScores.Count && !found; i++ )
			{
				if ( playerScores[i].netID == cl.NetworkIdent )
				{
					playerScores.RemoveAt( i );
					found = true;
				}
			}
		}
	}
}
