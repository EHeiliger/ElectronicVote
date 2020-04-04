using System;
using System.Collections.Generic;

namespace ElectronicVoteSystem.Models
{
    public partial class Election
    {
        public Election()
        {
            BallotPaper = new HashSet<BallotPaper>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateInit { get; set; }
        public DateTime DateEnd { get; set; }
        public int PositionId { get; set; }
        public bool Status { get; set; }

        public virtual Position Position { get; set; }
        public virtual ICollection<BallotPaper> BallotPaper { get; set; }
    }
}
