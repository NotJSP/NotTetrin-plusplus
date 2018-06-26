using NotTetrin.Constants;

namespace NotTetrin.Ingame.Single.Marathon {
    public class MarathonHighScore : HighScore {
        protected override string playerPrefsKey => PlayerPrefsKey.HighScore[RankingType.MarathonMode];
    }
}
