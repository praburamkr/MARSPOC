using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class AuthDbModel
    {
        public AuthDbModel(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UserId")]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string UserName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string Token { get; set; }

        /*[Column(TypeName = "nvarchar(250)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TimeStamp { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Expiration { get; set; }*/

    }
}
