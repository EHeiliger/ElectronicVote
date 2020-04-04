using System;
using System.Collections.Generic;

namespace ElectronicVoteSystem.Models
{
    public partial class BallotPaper
    {
        public BallotPaper()
        {
            Vote = new HashSet<Vote>();
        }

        public int Id { get; set; }
        public int ElectionId { get; set; }
        public bool? Status { get; set; }
        public int CandidateId { get; set; }

        public virtual Candidate Candidate { get; set; }
        public virtual Election Election { get; set; }
        public virtual ICollection<Vote> Vote { get; set; }
    }
}
