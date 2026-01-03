using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Application.Constants
{
    public static class ServiceTypes
    {
        public const string SonyPlayStation = "SonyPlayStation";
        public const string Darts = "Пикадо";
        public const string EightBallPool = "Билјард";
        public const string Fifa = "Фудбалче";

        public static readonly List<string> AllTypes = new()
        {
            SonyPlayStation,
            Darts,
            EightBallPool,
            Fifa
        };

        public static bool IsValid(string serviceType)
        {
            return AllTypes.Contains(serviceType);
        }
    }
}