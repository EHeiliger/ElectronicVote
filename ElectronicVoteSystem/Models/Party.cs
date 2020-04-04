using System;
using System.Collections.Generic;

namespace ElectronicVoteSystem.Models
{
    public partial class Party
    {
        public Party()
        {
            Candidate = new HashSet<Candidate>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string Color { get; set; }
        public string Logo { get; set; }

        public virtual ICollection<Candidate> Candidate { get; set; }
    }
}
