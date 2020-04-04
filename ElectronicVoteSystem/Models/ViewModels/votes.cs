using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicVoteSystem.Models.ViewModels
{
    public class votes
    {
       public votes(string name,int count)
       {
           CandidateName = name;
           VoteCount = count;

       }
        public string CandidateName { get; set; }
        public int VoteCount { get; set; }
    }
}
