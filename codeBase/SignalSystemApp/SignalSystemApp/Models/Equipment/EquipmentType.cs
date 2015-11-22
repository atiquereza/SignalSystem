using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SignalSystemApp.Models.Equipment
{
    public class EquipmentType
    {
        public int TypeID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<EquipmentDetails> Equipments { get; set; }
    }
}