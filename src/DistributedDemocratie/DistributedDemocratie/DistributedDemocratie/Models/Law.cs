using System.ComponentModel.DataAnnotations;

namespace DistributedDemocratie.Models
{
    public class Law
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Le titre est obligatoire")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La description est obligatoire")]
        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le texte complet est obligatoire")]
        [StringLength(50000, ErrorMessage = "Le texte ne peut pas dépasser 50000 caractères")]
        public string FullText { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le type de loi est obligatoire")]
        public LawType Type { get; set; }
        
        public LawStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? VotingStartDate { get; set; }
        public DateTime? VotingEndDate { get; set; }
        
        [Required(ErrorMessage = "Le nom du proposant est obligatoire")]
        [StringLength(100, ErrorMessage = "Le nom du proposant ne peut pas dépasser 100 caractères")]
        public string CreatedBy { get; set; } = string.Empty;
        
        public List<Vote> Votes { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        
        public int VotesFor => Votes.Count(v => v.Choice == VoteChoice.For);
        public int VotesAgainst => Votes.Count(v => v.Choice == VoteChoice.Against);
        public int VotesAbstain => Votes.Count(v => v.Choice == VoteChoice.Abstain);
        public int TotalVotes => Votes.Count;
        
        public bool IsVotingActive => Status == LawStatus.Voting 
            && VotingStartDate <= DateTime.Now 
            && (VotingEndDate == null || VotingEndDate > DateTime.Now);
    }

    public enum LawType
    {
        Law,
        Decree,
        Amendment,
        Referendum,
        LocalInitiative
    }

    public enum LawStatus
    {
        Draft,
        UnderReview,
        Voting,
        Passed,
        Rejected,
        Implemented
    }
}