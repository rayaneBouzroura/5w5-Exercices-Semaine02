using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO
{
    public class LoginSuccessDTO
    {

        [Required]
        public string Token { get; set; } = "";



    }
}
