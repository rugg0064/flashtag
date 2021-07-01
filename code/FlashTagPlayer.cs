using Sandbox;
using System;
using System.Collections.Generic;

namespace FlashTag
{
	public partial class FlashTagPlayer : Player
	{
		[Net]
		public bool isIt { get; set; }
		public float lastFlashedTime;
		float lastShotTime = -999999f;
		private List<ModelEntity> clothes;
		public FlashTagPlayer() : base()
		{
			lastFlashedTime = -9999.0f;
			clothes = new List<ModelEntity>();
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			Controller = new WalkController();

			Animator = new StandardPlayerAnimator();

			Camera = new FirstPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			SimulateActiveChild( cl, ActiveChild );

			if ( IsServer && Input.Pressed( InputButton.Attack1 ) && isIt && ((Time.Now - lastShotTime) > 0.5f) )
			{
				lastShotTime = Time.Now;
				FlashGrenade grenade = new FlashGrenade();
				grenade.SetModel( "models/m87.vmdl" );
				grenade.Position = EyePos + EyeRot.Forward * 40;
				grenade.Rotation = Rotation.LookAt( Vector3.Random.Normal );
				grenade.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
				grenade.PhysicsGroup.Velocity = EyeRot.Forward * 2048;
				grenade.Owner = this;
			}
		}

		public override void OnKilled()
		{
			base.OnKilled();

			//getFlashed();

			EnableDrawing = false;
		}

		[ClientRpc]
		public void getFlashed()
		{
			lastFlashedTime = Time.Now;
			this.PlaySound( "superflashnoise" );
		}
	}
}
