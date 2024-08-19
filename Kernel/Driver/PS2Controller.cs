namespace MOOS.Driver
{
    public static class PS2Controller
    {
        public static void Initialise()
        {
            PS2Keyboard.Initialise();
            PS2Mouse.Initialise();
        }
    }
}
