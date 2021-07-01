using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FlashTag
{
	[Library]
	public partial class FlashTagScoreboard : Panel
	{
		private IList<FlashTagScoreboardStruct> playerScores;
		private Dictionary<int, Panel> netIDToPanel;
		private Label totalPlayersLabel;
		private Label seedLabel;
		private Panel scores;

		public FlashTagScoreboard()
		{
			StyleSheet.Load( "UI/Scoreboard/FlashTagScoreboardStyle.scss" );
			AddClass( "scoreboard" );

			Panel canvas = Add.Panel( "canvas" );
			Panel header = canvas.Add.Panel( "header" );
			Label gamemode = header.Add.Label( "FlashTag", "gamemode" );
			totalPlayersLabel = header.Add.Label( "Total Players: X", "totalPlayers" );

			Panel content = canvas.Add.Panel( "Content" );
			Panel legend = content.Add.Panel( "legend" );
			legend.Add.Label( "Name", "name" );
			legend.Add.Label( "Times-tagged", "taggedCount" );

			scores = content.Add.Panel( "scores" );

			playerScores = new List<FlashTagScoreboardStruct>();
			netIDToPanel = new Dictionary<int, Panel>();
		}

		public override void Tick()
		{
			SetClass( "open", Input.Down( InputButton.Score ) );
			totalPlayersLabel.Text = "Total Players: " + Client.All.Count.ToString(); 
			IList<FlashTagScoreboardStruct> curScores = ((FlashTagGame)(Game.Current)).playerScores;
			(List<FlashTagScoreboardStruct> left, List<FlashTagScoreboardStruct> joined) changedScores = whatChanged( playerScores, curScores );

			List<FlashTagScoreboardStruct> list = new List<FlashTagScoreboardStruct>();
			foreach ( FlashTagScoreboardStruct s in curScores )
			{
				list.Add( s );
			}
			playerScores = list;

			foreach ( FlashTagScoreboardStruct s in changedScores.joined )
			{
				Panel p = scores.Add.Panel( "score" );
				p.Add.Label( getNameFromNetID( s.netID ), "name" );
				p.Add.Label( "0", "taggedCount" );
				//p.Add.Label( "0", "deaths" );
				//p.Add.Label( "R", "reserved" );
				//nameToPanel[s] = p;
				netIDToPanel[s.netID] = p;
			}
			foreach ( FlashTagScoreboardStruct s in changedScores.left )
			{
				Log.Info( "SOMEONE LEFT: " + s + " : " + getNameFromNetID( s.netID ) );
				netIDToPanel[s.netID].Delete();
			}
			//RN assume height = 10%
			List<FlashTagScoreboardStruct> sorted = sortList( playerScores );

			for ( int i = 0; i < sorted.Count; i++ )
			{
				//float percentShouldBe = ((9.93f * i) );
				float percentShouldBe = ((10.5f * i));
				Panel p = netIDToPanel[sorted[i].netID];
				Length? l = p.Style.Top;
				float curPercent = l.HasValue ? l.Value.Value : 0.0f;
				float changeAmount = curPercent - percentShouldBe;
				if ( MathF.Abs( changeAmount ) < 0.25f )
				{
					p.Style.Set( "top", percentShouldBe.ToString() + "%" );
				}
				else
				{
					p.Style.Set( "top", MathX.LerpTo( curPercent, percentShouldBe, 2.5f * Time.Delta ).ToString() + "%" );
				}
				((Label)p.GetChild( 1 )).Text = sorted[i].taggedCount.ToString();
			}
		}

		private List<FlashTagScoreboardStruct> sortList( IList<FlashTagScoreboardStruct> list )
		{
			List<FlashTagScoreboardStruct> newList = new List<FlashTagScoreboardStruct>( list );
			for ( int i = 0; i < newList.Count; i++ )
			{
				for ( int j = i + 1; j < newList.Count; j++ )
				{
					{
						if ( newList[i].taggedCount > newList[j].taggedCount )
						{
							FlashTagScoreboardStruct temp = newList[i];
							newList[i] = newList[j];
							newList[j] = temp;
						}
					}
				}
			}
			return newList;
		}

		private string getNameFromNetID( int netID )
		{
			IReadOnlyList<Client> clients = Client.All;
			for ( int i = 0; i < clients.Count; i++ )
			{
				if ( clients[i].NetworkIdent == netID )
				{
					return clients[i].Name;
				}
			}
			return null;
		}

		//Terrible efficiency!
		private (List<FlashTagScoreboardStruct> list1Exclusive, List<FlashTagScoreboardStruct> list2Exclusive) whatChanged( IList<FlashTagScoreboardStruct> list1, IList<FlashTagScoreboardStruct> list2 )
		{
			List<FlashTagScoreboardStruct> list1Exclusive = new List<FlashTagScoreboardStruct>();
			List<FlashTagScoreboardStruct> list2Exclusive = new List<FlashTagScoreboardStruct>();

			for ( int i = 0; i < list1.Count; i++ )
			{
				bool found = false;
				for ( int j = 0; j < list2.Count && !found; j++ )
				{
					//if (list1[i].Equals(list2[j]))
					if ( list1[i].netID == list2[j].netID )
					{
						found = true;
					}
				}
				if ( !found )
				{
					list1Exclusive.Add( list1[i] );
				}
			}

			for ( int i = 0; i < list2.Count; i++ )
			{
				bool found = false;
				for ( int j = 0; j < list1.Count && !found; j++ )
				{
					//if (list2[i].Equals(list1[j]))
					if ( list2[i].netID == list1[j].netID )
					{
						found = true;
					}
				}
				if ( !found )
				{
					list2Exclusive.Add( list2[i] );
				}
			}

			return (list1Exclusive, list2Exclusive);
		}
	}
}
