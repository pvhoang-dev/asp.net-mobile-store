using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BTL_QuanLyBanDienThoai.Models.ViewModel

{
    public class UserViewModel
    {
        [Key]
        public int? Id { get; set; }
        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(250)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is not in the correct format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [StringLength(250, MinimumLength = 8, ErrorMessage = "Password must have 8 characters")]
        public string? Password { get; set; }
        
        [StringLength(250, MinimumLength = 8, ErrorMessage = "Confirm password must have 8 characters")]
        public string? ConfirmPassword { get; set; }
       
        public int? Role { get; set; }
    }
}
