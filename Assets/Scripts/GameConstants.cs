using System;

[Serializable]
public class GameConstants
{
    public float BallSpeed = 3.5f;

    public int GemGenerationBlock = 5;
    
    public int EasyCorridorThickness = 3;
    public int MediumCorridorThickness = 2;
    public int HardCorridorThickness = 1;

    public int BallsPoolSize = 5;
    public int TilesPoolSize = 150;
    public int GemsPoolSize = 50;
    
    public int StartingPlatformSize = 3;
    public int VisibleTiles = 40;
    
    public Difficulty Difficulty;
    public bool RandomGems;
    
    public int CorridorSize
    {
        get
        {
            switch (Difficulty)
            {
                case Difficulty.Easy: return EasyCorridorThickness;
                case Difficulty.Medium: return MediumCorridorThickness;
                case Difficulty.Hard: return HardCorridorThickness;
                default: return 0;
            }
        }
    }
}