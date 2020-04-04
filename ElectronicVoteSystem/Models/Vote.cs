using System;
using System.Collections.Generic;

namespace ElectronicVoteSystem.Models
{
    public partial class Vote
    {
        public int Id { get; set; }
        public int BallotPaperId { get; set; }
        public string CitizenId { get; set; }
        public DateTime Date { get; set; }

        public virtual BallotPaper BallotPaper { get; set; }
        public virtual Citizen Citizen { get; set; }
    }
}
