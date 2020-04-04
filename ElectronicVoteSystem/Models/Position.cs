using System;
using System.Collections.Generic;

namespace ElectronicVoteSystem.Models
{
    public partial class Position
    {
        public Position()
        {
            Election = new HashSet<Election>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Election> Election { get; set; }
    }
}
