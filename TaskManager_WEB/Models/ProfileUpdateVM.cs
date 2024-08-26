namespace TaskManager_WEB.Models
{
    public class ProfileUpdateVM
    {

        public ProfileUpdateVM()
        {
            userUpdateDto = new UserUpdateDto();
        }

        public UserUpdateDto userUpdateDto { get; set; }
    }
}
