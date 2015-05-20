using System.Collections.Generic;

namespace Bridge.IDLL.Interfaces
{
    public interface IForexMarketPathRepository
    {

        string ForexTreesPath { get; set; }
        List<string> Paths { get; }

        void SetPaths(int startingMonth, string period, int startingChunk);
        int GetChunk(string filePath);

    }
}
