using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashTag
{
	public struct FlashTagScoreboardStruct
	{
		public int netID;
		public int taggedCount;

		public FlashTagScoreboardStruct(int netID)
		{
			this.netID = netID;
			this.taggedCount = 0;
		}

		public FlashTagScoreboardStruct giveTag()
		{
			return new FlashTagScoreboardStruct()
			{
				netID = this.netID,
				taggedCount = this.taggedCount + 1
			};
		}

		public override int GetHashCode()
		{
			return ((taggedCount << 16) + (taggedCount & 0xFFFF)) ^ netID;
		}

		public override bool Equals( object obj )
		{
			if ( !(obj is FlashTagScoreboardStruct) )
			{
				return false;
			}

			FlashTagScoreboardStruct other = (FlashTagScoreboardStruct)obj;
			return this.netID == other.netID &&
				this.taggedCount == other.taggedCount;
		}
	}
}
