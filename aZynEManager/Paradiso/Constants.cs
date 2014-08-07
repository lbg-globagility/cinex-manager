using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{
    public static class Constants
    {
        public const string DateTimeUiFormat = "MMMM dd, yyyy, ddd hh:mmtt";

        //in seconds
        public const int DateTimeUiInterval = 3;
        public const int MovieScheduleUiInterval = 10;
        public const int ReservedSeatingUiInterval = 5; //disable timer when creating transaction
    }
}
