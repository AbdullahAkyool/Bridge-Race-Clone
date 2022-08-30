
public enum GameState
{
    Start,
    Playing,
    Success,
    Fail

}

public static class StateMachine
{
    #region GameState

    public static bool isGamePlaying()
    {
        return GameManager.Instance.gameState == GameState.Playing;
    }
    public static bool isGameSucceed()
    {
        return GameManager.Instance.gameState == GameState.Success;
    }
    public static bool isGameFailed()
    {
        return GameManager.Instance.gameState == GameState.Fail;
    }

    #endregion

    public static void Initialize()
    {
        GameManager.Instance.gameState = GameState.Start;
    }
    public static void StartGame()
    {
        if (GameManager.Instance.gameState == GameState.Playing) return;
        GameManager.Instance.gameState = GameState.Playing;
    }
    public static void SuccessGame()
    {
        if (GameManager.Instance.gameState == GameState.Success) return;
        GameManager.Instance.gameState = GameState.Success;
    }
    public static void FailGame()
    {
        if (GameManager.Instance.gameState == GameState.Fail) return;
        GameManager.Instance.gameState = GameState.Fail;
    }
}
