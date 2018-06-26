using NotTetrin.Constants;

namespace NotTetrin.Ingame.Single.Stack {
    public class StackHighScore : HighScore {
        protected override string playerPrefsKey => PlayerPrefsKey.HighScore[RankingType.StackMode];
    }
}
