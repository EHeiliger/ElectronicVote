using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicVoteSystem.Models
{
    public class UserElection
    {
        
        public UserElection(string name, string electionName)
        {
            CandidateName = name;
            ElectionName = electionName;

        }
        public string CandidateName { get; set; }
        public string ElectionName { get; set; }
    }
}

