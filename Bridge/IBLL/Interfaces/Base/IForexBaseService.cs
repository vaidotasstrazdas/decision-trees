namespace Bridge.IBLL.Interfaces.Base
{
    public interface IForexBaseService
    {

        string Period { get; set; }
        int StartingMonth { get; set; }
        int StartingChunk { get; set; }

    }
}
