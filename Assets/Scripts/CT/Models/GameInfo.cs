using Zenject;

namespace CT.Models
{
    public class GameInfo
    {
        public int Score { get; private set; }

        public class Builder
        {

            [Inject]
            public Builder()
            {

            }
        }
    }
}
