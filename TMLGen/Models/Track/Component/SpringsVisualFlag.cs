namespace TMLGen.Models.Track.Component
{
    public enum SpringsVisualFlag : uint
    {
        Body = 1U,
        Armour = 2U,
        Weapon = 4U,
        Wings = 8U,
        Horns = 16U,
        Instrument = 32U,
        Tail = 64U,
        Hair = 128U,
        NakedBody = 256U,
        PrivateParts = 512U,
        Head = 32768U,
        AllArmour = 26U,
        AllCombat = 31U,
        AllVisuals = 4294967295U
    }
}
