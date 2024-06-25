using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ControlAdmin_os.Common.Entities
{
    public class City
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required]
        public string? Name { get; set; }

        [JsonIgnore] //Este campo no se creará en la base de datos, Sirve para saber a que departamento pertenece
        [NotMapped]
        public int IdDepartment { get; set; }
    }

}
