using DistributedDemocratie.Models;

namespace DistributedDemocratie.Services
{
    public interface ICitizenService
    {
        Task<Citizen?> GetCitizenByIdAsync(string id);
        Task<Citizen?> GetCitizenByEmailAsync(string email);
        Task<Citizen> RegisterCitizenAsync(Citizen citizen);
        Task<bool> VerifyCitizenAsync(string id);
        Task<List<Citizen>> GetAllCitizensAsync();
    }

    public class CitizenService : ICitizenService
    {
        private readonly List<Citizen> _citizens;

        public CitizenService()
        {
            _citizens = new List<Citizen>
            {
                new Citizen
                {
                    Id = "user_demo",
                    FirstName = "Jean",
                    LastName = "Dupont",
                    Email = "jean.dupont@example.com",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    IsVerified = true,
                    RegistrationDate = DateTime.Now.AddDays(-30),
                    Status = CitizenStatus.Active,
                    Regions = new List<string> { "Île-de-France", "France" }
                }
            };
        }

        public Task<Citizen?> GetCitizenByIdAsync(string id)
        {
            var citizen = _citizens.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(citizen);
        }

        public Task<Citizen?> GetCitizenByEmailAsync(string email)
        {
            var citizen = _citizens.FirstOrDefault(c => c.Email == email);
            return Task.FromResult(citizen);
        }

        public Task<Citizen> RegisterCitizenAsync(Citizen citizen)
        {
            citizen.Id = Guid.NewGuid().ToString();
            citizen.RegistrationDate = DateTime.Now;
            citizen.Status = CitizenStatus.Pending;
            _citizens.Add(citizen);
            return Task.FromResult(citizen);
        }

        public Task<bool> VerifyCitizenAsync(string id)
        {
            var citizen = _citizens.FirstOrDefault(c => c.Id == id);
            if (citizen != null)
            {
                citizen.IsVerified = true;
                citizen.Status = CitizenStatus.Active;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<List<Citizen>> GetAllCitizensAsync()
        {
            return Task.FromResult(_citizens.ToList());
        }
    }
}