using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ControlAdmin_os.Common.Entities
{
    public class Department
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string? Name { get; set; }

        public ICollection<City>? Cities { get; set; }

        [DisplayName("Cities Number")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;

        [JsonIgnore] //Este campo no se creará en la base de datos, Sirve para saber a que pais pertenece
        [NotMapped]
        public int IdCountry { get; set; }

       

    }


}
