namespace DistributedDemocratie.Models
{
    public class Citizen
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? FranceConnectId { get; set; }
        public bool IsVerified { get; set; }
        public DateTime RegistrationDate { get; set; }
        public CitizenStatus Status { get; set; }
        public List<string> Regions { get; set; } = new();
        
        // Historique des votes
        public List<Vote> VotingHistory { get; set; } = new();
        
        public bool CanVote => IsVerified && Status == CitizenStatus.Active && IsAdult;
        public bool IsAdult => DateTime.Now.Year - DateOfBirth.Year >= 18;
    }

    public enum CitizenStatus
    {
        Pending,
        Active,
        Suspended,
        Deactivated
    }
}