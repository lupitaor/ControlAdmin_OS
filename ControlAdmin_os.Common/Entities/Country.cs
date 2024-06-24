using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlAdmin_os.Common.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage ="El campo {0} no puede tener mas de {1} caracteres.")]
        [Required]
        public string? Name { get; set; }
    }

}
