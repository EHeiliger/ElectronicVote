using System;
using System.Collections.Generic;

namespace ElectronicVoteSystem.Models
{
    public partial class Citizen
    {
        public Citizen()
        {
            Candidate = new HashSet<Candidate>();
            Vote = new HashSet<Vote>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Candidate> Candidate { get; set; }
        public virtual ICollection<Vote> Vote { get; set; }
    }
}
