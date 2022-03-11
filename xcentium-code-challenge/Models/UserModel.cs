using System;
using System.ComponentModel.DataAnnotations;

namespace xcentium_code_challenge.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is Required.")]
        public String Username { get; set; }
        [Required(ErrorMessage = "Password is Required.")]
        public String Password { get; set; }
        public String Name { get; set; }
    }
}
