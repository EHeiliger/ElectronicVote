using System;
using System.Collections.Generic;

namespace ElectronicVoteSystem.Models
{
    public partial class Candidate
    {
        public Candidate()
        {
            BallotPaper = new HashSet<BallotPaper>();
        }

        public int Id { get; set; }
        public string CitizenId { get; set; }
        public int PartyId { get; set; }
        public string ProfileAvatar { get; set; }
        public bool Status { get; set; }

        public virtual Citizen Citizen { get; set; }
        public virtual Party Party { get; set; }
        public virtual ICollection<BallotPaper> BallotPaper { get; set; }
    }
}
