using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuth.Application.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Old password is required")]
        [MinLength(5, ErrorMessage = "Old password must be at least 5 characters")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [MinLength(5, ErrorMessage = "New password must be at least 5 characters")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
