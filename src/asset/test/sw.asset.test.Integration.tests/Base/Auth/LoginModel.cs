using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dottrack.asset.Integration.tests.Base.Auth;

public class LoginModel
{
    [Editable(true)]
    [Required(AllowEmptyStrings = false)]
    public string Login { get; set; }
    [Required(AllowEmptyStrings = false)]
    [Editable(true)]
    public string Password { get; set; }
}
