using System;
using System.Collections.Generic;

namespace WebApi.Models.Completation
{
    public class CompletationOrder
    {
        public int id { get; set; }
        public string value { get; set; }
        public string orderNumber { get; set; }
        public string distributionCenter { get; set; }
        public DateTime realiseBy { get; set; }

        public string departureRamp { get; set; }
        public DateTime departureTime { get; set; }
        public string departureCarId { get; set; }

        public IEnumerable<CompletationPalletPackAction> pallets { get; set; }

        public bool closed { get; set; }
        public int closedByOperatorId { get; set; }
        public DateTime closingTimestamp { get; set; }
        public string closingSignature { get; set; }
    }
}
