using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dottrack.asset.Integration.tests.Base.Auth
{
    public class AuthModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(false)]
        public string Token { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(false)]
        public string RefreshToken { get; set; }
    }
}
