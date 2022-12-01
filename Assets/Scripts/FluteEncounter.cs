using MMM.DialogSystem;

namespace MMM
{
    public class FluteEncounter : Encounter
    {
        public PlayerMovement PlayerMovement;

        protected override void OnSongPass()
        {
            base.OnSongPass();
            PlayerMovement.AllowDoubleJump();
        }
    }
}