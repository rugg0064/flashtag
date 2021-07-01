using Sandbox;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FlashTag
{
	class MySuperNoiseDumbAssSound
	{
		static public SoundEvent AttackSound = new( "sounds/supernoise.sound" );
	}

	public class FlashGrenade : ModelEntity
	{
		private float creationTime;
		private FlashTagGame game;
		private static SoundEvent soundEvent;
		public FlashGrenade() : base()
		{
			this.creationTime = Time.Now;
			this.game = (FlashTagGame)Game.Current;
		}

		[Event.Tick.Server]
		private void tick()
		{
			//Log.Info( Time.Now - creationTime );
			this.PlaySound( "superFlashExplosionNoise" );
			if ( Time.Now - creationTime > 1.5f)
			{
				this.Delete();
				FlashTagPlayer[] players = All.OfType<FlashTagPlayer>().ToArray();
				List<FlashTagPlayer> flashedPlayers = new List<FlashTagPlayer>();
				for ( int i = 0; i < players.Length; i++ )
				{
					FlashTagPlayer player = players[i];
					Camera camera = (Camera)player.Camera;
					Vector3 eyePos = player.EyePos;
					Vector3 eyeDir = player.EyeRot.Forward;
					Vector3 eyeToNade = this.Position - eyePos;
					float dot = Vector3.Dot( eyeToNade.Normal, eyeDir.Normal );
					if(dot > 0.65f)
					{
						TraceResult rayCast = Trace.Ray( this.Position, eyePos ).Run();
						if ( rayCast.Entity == player)
						{
							flashedPlayers.Add( player );
						}
					}
				}

				if ( flashedPlayers.Count > 0 )
				{
					int flashedIndex = Rand.Int( 0, flashedPlayers.Count - 1 );
					//Log.Info( flashedIndex );
					game.resetAllIts();
					game.setIt( flashedPlayers[flashedIndex] );

					for ( int i = 0; i < flashedPlayers.Count; i++ )
					{
						FlashTagPlayer player = flashedPlayers[i];
						game.gotFlashed( this, player );
						//player.unDress();
					}
				}
			}
		}
	}
}
