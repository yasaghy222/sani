using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Areas
{
    public class Account
    {
        public class LoginVar
        {
            [Required(ErrorMessage ="این فیلد اجباری است.")]
            [RegularExpression(pattern:@"^09[0-9]{9}$",ErrorMessage ="فرمت ورودی صحیح نمی باشد.")]
            public string phoneNumber { get; set; }
            [Required(ErrorMessage = "این فیلد اجباری است.")]
            public string passWord { get; set; }
        }

        public class ForgetPassVar
        {
            public string validationCode { get; set; }
        }
    }
}
