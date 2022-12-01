namespace MMM.Music
{
    public readonly struct Note
    {
        public readonly int Time;
        public readonly int Lane;

        public Note(int time, int lane)
        {
            Time = time;
            Lane = lane;
        }
    }
}