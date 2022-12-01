namespace MMM
{
    public class DrumEncounter : Encounter
    {
        public DrumThrower DrumThrower;
        
        protected override void OnSongPass()
        {
            base.OnSongPass();
            DrumThrower.enabled = true;
        }
    }
}
