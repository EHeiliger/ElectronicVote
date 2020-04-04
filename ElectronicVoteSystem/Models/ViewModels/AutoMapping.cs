using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace ElectronicVoteSystem.Models.ViewModels
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            ConfigureCandidate();
            ConfigureParty();
            ConfigureElection();
        }

        private void ConfigureCandidate()
        {
            CreateMap<CandidateViewModel, Candidate>();
            CreateMap<Candidate, CandidateViewModel>().ForMember(dest => dest.ProfileAvatar, opt => opt.Ignore());
        }

        private void ConfigureParty()
        {

            CreateMap<PartyViewModel, Party>().ForMember(dest => dest.Logo, opt => opt.Ignore()); ;/*.ForMember(dest => dest.Id, opt => opt.UseDestinationValue());*/
            // CreateMap<Party, PartyViewModel>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Party, PartyViewModel>().ForMember(dest => dest.Logo, opt => opt.Ignore());

        }

        private void ConfigureElection()
        {
            CreateMap<ElectionViewModel, Election>();
            CreateMap<Election, ElectionViewModel>().ForMember(dest => dest.DateInit, opt => opt.Ignore());
            CreateMap<Election, ElectionViewModel>().ForMember(dest => dest.InitTime, opt => opt.Ignore());
            CreateMap<Election, ElectionViewModel>().ForMember(dest => dest.DateEnd, opt => opt.Ignore());
            CreateMap<Election, ElectionViewModel>().ForMember(dest => dest.EndTime, opt => opt.Ignore());

            //CreateMap<Party, PartyViewModel>().ForMember(dest => dest.Logo, opt => opt.Ignore());

        }

    }
}
