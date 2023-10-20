using Infrastructure.Services.Input.Main.Core;

namespace Entities.Player
{
    public class PlayerMovement
    {
        private readonly IMainInputService _inputService;

        public PlayerMovement(IMainInputService inputService)
        {
            _inputService = inputService;
        }
        
        
    }
}