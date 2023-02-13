using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthenticationSystemV2.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        
        [MaxLength(125)]
        [EmailAddress]
        [Required]
        [Column(TypeName = "varchar(10)")]
        public string Email { get; set; }
        
        [MaxLength(125)]
        [Required]
        [Column(TypeName = "varchar(10)")]
        public string Password { get; set; }
        

        public ICollection<System.Threading.Tasks.Task> Tasks;

        public User(Guid id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;

            Tasks = new List<System.Threading.Tasks.Task>();
        }

        public User()
        {
            
        }
    }
}