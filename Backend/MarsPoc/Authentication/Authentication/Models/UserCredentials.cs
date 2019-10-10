using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class UserCredentialsModel:IModel
    {
        [NotMapped]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public void  Copy(IModel item)
        {

        }


    }
}
