using GameStore.Abstractions;

namespace GameStore.WebApi.Configurations
{
    public class TokenParameters: ITokensParameters
    {
        public string UserName { get; set; }
        public string PaswordHash { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
    }
}
