using Contracts.Session;

namespace Business.Tests
{
    internal abstract class BusinessTestBase
    {
        protected PlayerPrincipal InitialPrincipal { get; private set; }

        protected BusinessTestBase()
        {
            // Create fake session
            InitialPrincipal = new PlayerPrincipal("fakeUser")
            {
                Id = 1,
                UserName = "fakeUserName",
                IsAdmin = true
            };

            Session.Current = InitialPrincipal;
        }
    }
}
