using System.Linq;
using ew = Cinex.Core.Entities.Ewallet;

namespace Paradiso.EF.Ewallet
{
    internal class EwalletDto
    {
        public int? Id { get; private set; }

        public string Name { get; private set; }

        public string AccountNo { get; private set; }

        public bool IsActive { get; private set; }

        public bool IsDefault { get; set; }

        private readonly ew _eWallet;

        public ew Ewallet => _eWallet;

        private EwalletDto(string name)
        {
            Name = name;
        }

        public EwalletDto(ew ewallet)
        {
            _eWallet = ewallet;

            Id = ewallet.Id;
            Name = ewallet.Name;
            AccountNo = ewallet.AccountNo;
            IsActive = ewallet.IsActive;
            IsDefault = ewallet.IsDefault;
        }

        public static EwalletDto NonEwallet(string name = "NON-EWALLET") => new EwalletDto(name);

        public string DisplayText
        {
            get
            {
                string[] texts = { Name, AccountNo };

                var fsdfsd = texts.Where(t => !string.IsNullOrEmpty(t)).ToArray();

                return string.Join(" - ", fsdfsd);
            }
        }
    }
}
