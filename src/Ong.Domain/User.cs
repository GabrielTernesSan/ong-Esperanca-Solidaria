namespace Ong.Domain
{
    public class User
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; }
        public string Cpf { get; private set; }

        public User(string name, string email, string passwordHash, string role, string cpf)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio ou nulo.", nameof(email));
            if (!IsValidEmail(email))
                throw new ArgumentException("Formato de email é inválido", nameof(email));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash não pode ser vazio ou nulo.", nameof(passwordHash));
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role não pode ser vazio ou nulo.", nameof(role));
            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("CPF não pode ser vazio ou nulo.", nameof(cpf));

            var normalizedCpf = NormalizeCpf(cpf);
            if (!IsValidCpf(normalizedCpf))
                throw new ArgumentException("CPF inválido.", nameof(cpf));

            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            Cpf = normalizedCpf;
        }

        public User(Guid id, string name, string email, string passwordHash, string role, string cpf)
            : this(name, email, passwordHash, role, cpf)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id não pode ser vazio ou nulo.", nameof(id));

            Id = id;
        }

        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash não pode ser vazio ou nulo.", nameof(passwordHash));

            PasswordHash = passwordHash;
        }

        public void SetRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role não pode ser vazio ou nulo.", nameof(role));

            Role = role;
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Email não pode ser vazio ou nulo.", nameof(newEmail));
            if (!IsValidEmail(newEmail))
                throw new ArgumentException("Formato de email é inválido (Parameter 'newEmail')", nameof(newEmail));

            Email = newEmail;
        }

        public static string NormalizeCpf(string cpf)
            => new string(cpf.Where(char.IsDigit).ToArray());

        public static bool IsValidCpf(string cpf)
        {
            var normalized = NormalizeCpf(cpf);

            if (normalized.Length != 11)
                return false;

            if (normalized.Distinct().Count() == 1)
                return false;

            var sum = 0;
            for (int i = 0; i < 9; i++)
                sum += (normalized[i] - '0') * (10 - i);
            var remainder = sum % 11;
            var digit1 = remainder < 2 ? 0 : 11 - remainder;
            if ((normalized[9] - '0') != digit1)
                return false;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += (normalized[i] - '0') * (11 - i);
            remainder = sum % 11;
            var digit2 = remainder < 2 ? 0 : 11 - remainder;
            return (normalized[10] - '0') == digit2;
        }

        public static string MaskCpf(string normalizedCpf)
            => $"***.***.***-{normalizedCpf[9]}{normalizedCpf[10]}";

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
