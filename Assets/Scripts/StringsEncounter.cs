namespace MMM
{
    public class StringsEncounter : Encounter
    {
        public PlayerMovement PlayerMovement;
        
        protected override void OnSongPass()
        {
            base.OnSongPass();
            PlayerMovement.AllowGravityFlip();
        }
    }
}