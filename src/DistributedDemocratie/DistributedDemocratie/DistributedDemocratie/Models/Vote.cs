namespace DistributedDemocratie.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int LawId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public VoteChoice Choice { get; set; }
        public DateTime VotedAt { get; set; }
        public string? Comment { get; set; }
        public bool IsVerified { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        
        // Pour la traçabilité blockchain future
        public string? BlockchainHash { get; set; }
        public string? PreviousVoteHash { get; set; }
    }

    public enum VoteChoice
    {
        For,
        Against,
        Abstain
    }
}