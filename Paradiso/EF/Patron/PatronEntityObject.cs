using System.Data.Objects.DataClasses;

namespace Paradiso
{
	public partial class patron : EntityObject
	{
		public const decimal LOCAL_AMUSEMENT_TAX_RATE = 0.05m;
		public const decimal FOREIGN_AMUSEMENT_TAX_RATE = 0.10m;

		public decimal GetAmusementTaxAmount(decimal grossTicketPrice) => grossTicketPrice * (amusement_tax_rate / (1 + amusement_tax_rate));

		public decimal GetCulturalTaxAmount(decimal grossTicketPrice) => grossTicketPrice * (cultural_tax_rate / (1 + cultural_tax_rate));
	}
}