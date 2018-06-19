namespace SM.Sound
{
    public enum InterruptionType
    {
        DontCare, //plays sound regardless if the same sound is playing or not
        Interrupt, //stops instances of the same sound and plays again from beginning
        DontInterrupt, //if an instance of the sound is playing, dont play the sound
        DontInterruptButInterruptOthers //doesnt interrupt this sound, but does interrupt other sounds
    }
}